using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Humanizer;

namespace LIMS_PaiementBack.Repositories
{
    public class DemandeRepository : IDemandeRepository
    {
        private readonly DbContextEntity _dbContext;

        public DemandeRepository(DbContextEntity dbContext)
        {
            _dbContext = dbContext;
        }

        // ajout de nouveau demande de note de débit
        /*
            * Cette méthode permet d'ajouter une nouvelle demande de note de débit.
            * Elle prend en paramètre un objet DemandeEntity contenant les informations de la demande.
            * Elle génère également un PDF à partir des données fournies.
            * 
            * @param demande : L'objet DemandeEntity contenant les informations de la demande.
            * @return : Tâche asynchrone qui retourne un tableau d'octets représentant le PDF généré.
            * @throws : Exception si une erreur se produit lors de l'exécution de la requête.
        */
        public async Task<List<byte[]>> AddDemandeAsync(DemandeEntity demande, DemandeDto demandeDto)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // 1. Récupérer la dernière référence dans Depart (en tant qu'entier)
                    int lastRef = await _dbContext.Depart
                        .OrderByDescending(d => d.reference)
                        .Select(d => d.reference)
                        .FirstOrDefaultAsync();

                    int newReference = lastRef + 1;

                    // 2. Mise à jour de l'état de la prestation pour la demande de note de débit
                    await _dbContext.Prestation
                        .Where(p => p.id_prestation == _dbContext.Etat_decompte
                            .Where(e => e.id_etat_decompte == demande.id_etat_decompte)
                            .Select(e => e.id_prestation)
                            .FirstOrDefault())
                        .ExecuteUpdateAsync(setters => setters.SetProperty(p => p.demandeEffectuer, true));            

                    // 3. Récupérer les infos pour créer le départ
                    var infos = await (
                        from ed in _dbContext.Etat_decompte
                        join p in _dbContext.Prestation on ed.id_prestation equals p.id_prestation
                        join c in _dbContext.Client on p.id_client equals c.id_client
                        where ed.id_etat_decompte == demande.id_etat_decompte
                        select new
                        {
                            ReferenceEtatDecompte = ed.ReferenceEtatDecompte,
                            NomClient = c.Nom,
                        }).FirstOrDefaultAsync();

                    if (infos == null)
                        throw new Exception("Impossible de récupérer les informations liées à la demande.");

                    // 4. Récupérer l'identifiant du destinataire
                    int id_destinataire = await _dbContext.Destinataire
                        .Where(d => d.designation == "Département Administratif et Financier")
                        .Select(c => c.idDestinataire)
                        .FirstOrDefaultAsync();

                    // 5. Créer l'objet DepartEntity
                    var depart = new DepartEntity
                    {
                        reference = newReference,
                        objet = $"Demande d'Etablissement de note de débit {infos.ReferenceEtatDecompte} au nom de {infos.NomClient} d'un montant de {demande.montant} ar",
                        DateDepart = DateTime.Now,
                        idDestinataire = id_destinataire
                    };

                    // 6. Affecter la référence à la demande
                    demande.reference = newReference;

                    // 7. Enregistrer demande et départ
                    await _dbContext.DemandeNoteDebit.AddAsync(demande);
                    await _dbContext.Depart.AddAsync(depart);
                    await _dbContext.SaveChangesAsync();

                    var travauxInfos = await (
                            from typeTravaux in _dbContext.Type_travaux
                            join detailEtatDecompte in _dbContext.Details_etat_decompte 
                                on typeTravaux.id_type_travaux equals detailEtatDecompte.id_type_travaux
                            join etatDecompte in _dbContext.Etat_decompte 
                                on detailEtatDecompte.id_etat_decompte equals etatDecompte.id_etat_decompte
                            join prestation in _dbContext.Prestation 
                                on etatDecompte.id_prestation equals prestation.id_prestation
                            join echantillon in _dbContext.Echantillon 
                                on prestation.id_prestation equals echantillon.id_prestation
                            join typeEchantillon in _dbContext.Type_echantillon 
                                on echantillon.id_type_echantillon equals typeEchantillon.id_type_echantillon
                            join typeTravauxTypeEchantillon in _dbContext.Type_travaux_type_echantillon
                                on new { echantillon.id_type_echantillon, typeTravaux.id_type_travaux }
                                equals new { typeTravauxTypeEchantillon.id_type_echantillon, typeTravauxTypeEchantillon.id_type_travaux }
                            where 
                                etatDecompte.id_etat_decompte == demande.id_etat_decompte
                            group detailEtatDecompte by new 
                            { 
                                typeTravaux.designation, 
                                detailEtatDecompte.prix_unitaire 
                            } into g
                            select new TravauxInfo
                            {
                                Designation = g.Key.designation,
                                Nombre = g.Count(),
                                PrixUnitaire = g.Key.prix_unitaire
                            }).ToListAsync();

                    // 8. Générer les PDF (hors transaction car ce n'est pas en base)
                    byte[] pdfBytes = FonctionGlobalUtil.GenerateDemandePdf(demandeDto, newReference);
                    byte[] pdfBytes2 = FonctionGlobalUtil.GenerateNoteDebitPdf(demandeDto, travauxInfos);

                    List<byte[]> pdfBytesList = new List<byte[]>();
                    pdfBytesList.Add(pdfBytes);
                    pdfBytesList.Add(pdfBytes2);

                    // 9. Valider la transaction
                    await transaction.CommitAsync();

                    return pdfBytesList;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        // liste des demande de note de débit éffectuer
        /*
            * Cette méthode permet de récupérer toutes les demandes de note de débit effectuées.
            * Elle effectue une jointure entre les tables Etat_decompte et DemandeNoteDebit.
            * 
            * @return : Tâche asynchrone qui retourne un objet ApiResponse contenant la liste des demandes.
            *          ou un message d'erreur si aucune demande n'est trouvée.
            * @throws : Exception si une erreur se produit lors de l'exécution de la requête.
        */
        public async Task<ApiResponse> GetAllDemandeAsync()
        {
            /*
                * Récupérer toutes les demandes de note de débit effectuées
                * Effectuer une jointure entre les tables Etat_decompte et DemandeNoteDebit
                * Trier les résultats par date de demande décroissante
                * Retourner la liste des demandes sous forme d'ApiResponse
            */
            var demandeList = await (
                from etat_decompte in _dbContext.Etat_decompte
                join demande in _dbContext.DemandeNoteDebit on etat_decompte.id_etat_decompte equals demande.id_etat_decompte
                orderby demande.DateDemande descending
                select new DemandeDto
                {
                    reference = demande.reference,
                    dateDemande = demande.DateDemande,
                    objet = demande.objet,
                    montant = demande.montant,
                    montant_literal = demande.MontantLiteral,
                    referenceEtatDecompte = etat_decompte.ReferenceEtatDecompte,
                    date_etat_decompte = etat_decompte.date_etat_decompte,
                    id_etat_decompte = etat_decompte.id_etat_decompte
                }).ToListAsync();

            var rendu = new ApiResponse
            {
                Data = demandeList,
                Message = demandeList.Any() ? "succes" : "Aucune donnée trouvé",
                IsSuccess = true,
                StatusCode = 200
            };
            return rendu;
        }

        // affichage des informations de demande de note de débit suivant la procédure normal
        /*
            * Cette méthode permet de récupérer les informations de demande de note de débit
            * en fonction de l'état de décompte spécifié.
            * Elle effectue une jointure entre les tables Client, Prestation, Etat_decompte et Echantillon.
            * 
            * @param id_etat_decompte : L'identifiant de l'état de décompte.
            * @return : Tâche asynchrone qui retourne un objet ApiResponse contenant la liste des demandes.
            *          ou un message d'erreur si aucune demande n'est trouvée.
            * @throws : Exception si une erreur se produit lors de l'exécution de la requête.
        */
        public async Task<ApiResponse> GetDemandesAsync(int id_etat_decompte)
        {
            var type = await _dbContext.Echantillon
                .Where(e => e.prestation.EtatDecompte.id_etat_decompte == id_etat_decompte)
                .GroupBy(e => e.typeEchantillon.designation)
                .Select(g => g.Key) // Prend uniquement la designation
                .ToListAsync();

            var travaux = await _dbContext.Details_etat_decompte
                .Where(d => d.EtatDecompte != null 
                        && d.EtatDecompte.id_etat_decompte == id_etat_decompte
                        && d.TypeTravaux != null)
                .Select(d => d.TypeTravaux.designation)
                .Distinct()
                .ToListAsync();

            var demande = await (from client in _dbContext.Client
                join prestation in _dbContext.Prestation on client.id_client equals prestation.id_client
                join etatDecompte in _dbContext.Etat_decompte on prestation.id_prestation equals etatDecompte.id_prestation
                join echantillon in _dbContext.Echantillon on prestation.id_prestation equals echantillon.id_prestation into echantillonsGroup
                where etatDecompte.id_etat_decompte == id_etat_decompte
                select new DemandeDto
                {
                    clients = client.Nom,
                    email = client.Email,
                    adresse = client.Adresse,
                    contact = client.Contact,
                    identite = FonctionGlobalUtil.GetClientIdentity(client.CIN ?? "", client.Passport ?? ""), // récuperation de d'identité du client
                    etatDecompte = etatDecompte.ReferenceEtatDecompte,
                    datePaiement = DateTime.Now,
                    id_etat_decompte = etatDecompte.id_etat_decompte,
                    montant = FonctionGlobalUtil.MontantReel(etatDecompte.total_montant, etatDecompte.remise),// récuperation du montant réel à payer
                    nombreEchantillon = echantillonsGroup.Count(),
                    montant_literal = FonctionGlobalUtil.ConvertirMontantEnLettres(etatDecompte.total_montant, etatDecompte.remise),
                    objet = FonctionGlobalUtil.GetObjetEchantillon(type),
                    travaux = FonctionGlobalUtil.GetTravaux(travaux) // récuperation de la liste des travaux
                }).ToListAsync();                      

            var result = new ApiResponse
            {
                Data = demande,
                Message = demande.Any() ? "Succès" : "Aucune donnée trouvée",
                IsSuccess = demande.Any(),
                StatusCode = 200
            };

            return result;
        }

        // liste des demandes de note de débit à faire 
        /*
            * Cette méthode permet de récupérer la liste des demandes de note de débit à faire.
            * Elle effectue une jointure entre les tables Etat_prestation, Prestation et Etat_decompte.
            * Elle filtre les résultats pour ne garder que ceux dont l'état de prestation est égal à 2 dans statuspaiement.
            * et les trie par date d'état de décompte décroissante.

            * @return : Tâche asynchrone qui retourne un objet ApiResponse contenant la liste des demandes.
            *          ou un message d'erreur si aucune demande n'est trouvée.
            * @throws : Exception si une erreur se produit lors de l'exécution de la requête.
        */
        public async Task<ApiResponse> GetListeEtatDecomptePayer()
        {
            // Récupération de la liste des demandes de note de débit à faire
            // en effectuant une jointure entre les tables Etat_prestation, Prestation et Etat_decompte
            var liste = await (
                /*
                    from etat_prestation in _dbContext.Etat_prestation
                    join prestation in _dbContext.Prestation on etat_prestation.id_etat_prestation equals prestation.id_etat_prestation
                    join etat_decompte in _dbContext.Etat_decompte on prestation.id_prestation equals etat_decompte.id_prestation
                    where etat_prestation.id_etat_prestation == 3
                */
                from prestation in _dbContext.Prestation
                join etat_decompte in _dbContext.Etat_decompte on prestation.id_prestation equals etat_decompte.id_prestation
                where prestation.status_paiement == true // prestation.status_paiement == false
                    && prestation.demandeEffectuer == false
                orderby etat_decompte.date_etat_decompte descending
                select new
                {
                    id_etat_decompte = etat_decompte.id_etat_decompte,
                    referenceEtatDecompte = etat_decompte.ReferenceEtatDecompte,
                    date_etat_decompte = etat_decompte.date_etat_decompte,
                    montant = FonctionGlobalUtil.MontantReel(etat_decompte.total_montant, etat_decompte.remise)// récuperation du montant réel à payer
                }).ToListAsync();

            return new ApiResponse
            {
                Data = liste,
                Message = liste.Any() ? "succès" : "aucune donnée trouvé",
                IsSuccess = true,
                StatusCode = 200
            };
        }

        // vérification des demande de note de débit non éffectuer
        /*
            * Cette méthode permet de vérifier les demandes de note de débit non effectuées.
            * Elle effectue une jointure entre les tables Prestation et Etat_decompte.
            * Elle filtre les résultats pour ne garder que ceux dont l'état de prestation est égal à 2 dans statuspaiement.
            * 
            * @return : Tâche asynchrone qui retourne un objet ApiResponse contenant la liste des demandes.
            *          ou un message d'erreur si aucune demande n'est trouvée.
            * @throws : Exception si une erreur se produit lors de l'exécution de la requête.
        */
        public async Task<ApiResponse> GetVerificationAsync()
        {
            // Date fixe pour les tests, sinon utiliser DateTime.Today
            DateTime today = DateTime.Today;
            //DateTime today = new DateTime(2025, 01, 21);

            // 1️⃣ Récupérer les ID des EtatDecompte du jour
            var etatDecompteJour = await (
                /*
                    from etat_prestation in _dbContext.Etat_prestation
                    join prestation in _dbContext.Prestation on etat_prestation.id_etat_prestation equals prestation.id_etat_prestation
                    join etat_decompte in _dbContext.Etat_decompte on prestation.id_prestation equals etat_decompte.id_prestation
                    where etat_prestation.id_etat_prestation == 3 && etat_decompte.date_etat_decompte == today
                */
                from prestation in _dbContext.Prestation
                join etat_decompte in _dbContext.Etat_decompte on prestation.id_prestation equals etat_decompte.id_prestation
                where prestation.status_paiement == true // prestation.status_paiement == false
                    && prestation.demandeEffectuer == false 
                    && etat_decompte.date_etat_decompte == today
                orderby etat_decompte.date_etat_decompte descending
                select new
                {
                    id_etat_decompte = etat_decompte.id_etat_decompte,
                    referenceEtatDecompte = etat_decompte.ReferenceEtatDecompte,
                    date_etat_decompte = etat_decompte.date_etat_decompte,
                    montant = FonctionGlobalUtil.MontantReel(etat_decompte.total_montant, etat_decompte.remise)
                }).ToListAsync();

            if (!etatDecompteJour.Any())
            {
                return new ApiResponse
                {
                    Data = null,
                    Message = "Aucun état de décompte trouvé pour aujourd'hui.",
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            // 2️⃣ Récupérer les ID déjà enregistrés dans DemandeNoteDebit
            var idDejaEnregistre = await _dbContext.DemandeNoteDebit
                .Where(d => d.DateDemande.HasValue && d.DateDemande.Value.Date == today)
                .Select(d => d.id_etat_decompte)
                .ToListAsync();

            // 3️⃣ Filtrer ceux qui ne sont pas encore enregistrés
            var Manquant = etatDecompteJour
                .Where(e => !idDejaEnregistre.Contains(e.id_etat_decompte))
                .Select(e => new DemandeDto
                {
                    id_etat_decompte = e.id_etat_decompte,
                    referenceEtatDecompte = e.referenceEtatDecompte,
                    dateDemande = e.date_etat_decompte,
                    montant = e.montant
                }).ToList();

            return new ApiResponse
            {
                Data = Manquant,
                Message = Manquant.Any() ? "⚠️ Vous avez oublié de faire ces demandes !" : "✅ Rien oublié.",
                IsSuccess = true,
                StatusCode = 200
            };
        }
    }
}
