using LIMS_PaiementBack.Utils;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks.Dataflow;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LIMS_PaiementBack.Repositories
{
    public class DelaiRepository : IDelaiRepository
    {
        private readonly DbContextEntity _dbContext;

        public DelaiRepository(DbContextEntity dbContext)
        {
            _dbContext = dbContext;
        }

        //ajout d'un nouveau delai accorder
        public async Task AddDelaiPaiement(DelaiEntity delai, PaiementEntity paiement)
        {
            await _dbContext.Paiement.AddAsync(paiement);
            await _dbContext.SaveChangesAsync();

            delai.idPaiement = paiement.idPaiement;

            await _dbContext.DelaiPaiement.AddAsync(delai);
            await _dbContext.SaveChangesAsync();

            await _dbContext.Prestation
                .Where(prestation =>
                    _dbContext.Etat_decompte
                        .Where(ed => _dbContext.Paiement
                            .Where(p => p.idPaiement == paiement.idPaiement)
                            .Select(p => p.id_etat_decompte)
                            .Contains(ed.id_etat_decompte)
                        )
                        .Select(ed => ed.id_prestation)
                        .Contains(prestation.id_prestation)
                )
                .ExecuteUpdateAsync(setters => setters.SetProperty(p => p.status_paiement, 4));
        }

        //afficher tout les delai qui ont été accorder
        public async Task<ApiResponse> GetAllDelaiAsync()
        {
            var delaiList = await (
                from etat_decompte in _dbContext.Etat_decompte
                join paiement in _dbContext.Paiement on etat_decompte.id_etat_decompte equals paiement.id_etat_decompte
                join delai_paiement in _dbContext.DelaiPaiement on paiement.idPaiement equals delai_paiement.idPaiement    
                select new DelaiDto
                {  
                    id_etat_decompte = etat_decompte.id_etat_decompte,
                    idPaiement = paiement.idPaiement,
                    referenceEtatDecompte = etat_decompte.ReferenceEtatDecompte,
                    date_etat_decompte = etat_decompte.date_etat_decompte,
                    datePaiement = paiement.DatePaiement,
                    EtatPaiement = paiement.EtatPaiement,
                    DateFinDelai = delai_paiement.DateFinDelai,
                    modePaiement = delai_paiement.ModePaiement
                }).ToListAsync();

            var rendu = new ApiResponse
            {
                Data = delaiList,
                Message = delaiList.Any() ? "succes" : "aucun donnée trouvé",
                IsSuccess = true,
                StatusCode = 200
            };
            return rendu;
        }

        //recherche des conditions d'accord du délai de paiement 
        /*
         Condition:
            1. Avoir fait des testes de plus 600 échantillon ou plus 
            2. Etre dans la période de 6 mois si c'est suffisant sinon voir 1 an 
            3. les clients sous-contrat pas de test pour accorder un délai
         */
        /*
            * Récupère les informations de validation du délai de paiement pour un client donné.
            * 
            * @param id_etat_decompte L'ID de l'état de décompte du client.
            * @returns Une tâche représentant l'opération asynchrone, contenant un ApiResponse avec les informations de validation du délai.
        */
        public async Task<ApiResponse> GetValidationDelai(int id_etat_decompte)
        {
            var dernierIdEtatDecompte = id_etat_decompte;

            // recherche du client via l'id_etat_decompte
            // Recherche du client en fonction d’un identifiant d’état de décompte (dernierIdEtatDecompte)
            var clientIdentity = await (
                from client in _dbContext.Client
                join prestation in _dbContext.Prestation on client.id_client equals prestation.id_client
                join etatDecompte in _dbContext.Etat_decompte on prestation.id_prestation equals etatDecompte.id_prestation
                where etatDecompte.id_etat_decompte == dernierIdEtatDecompte
                select new
                {
                    Nom = client.Nom,
                    Adresse = client.Adresse,
                    Identity = FonctionGlobalUtil.GetClientIdentity(client.CIN, client.Passport), // récupère l'identité depuis CIN ou Passeport
                    ref_contrat = client.ref_contrat // référence du contrat lié au client
                }).FirstOrDefaultAsync(); // récupère le premier résultat trouvé ou null si aucun


            if (clientIdentity == null)
            {
                return new ApiResponse
                {
                    Data = null,
                    Message = "Client introuvable dans le système, délai non accordé.",
                    IsSuccess = false,
                    StatusCode = 404
                };
            }
            else
            {
                // Vérifier si le client est sous contrat
                /*
                    faire un teste si oui ou non la colonne ref_contrat n'est pas vide mais presente un autre caractere que ""
                */
                bool estSousContrat = !string.IsNullOrWhiteSpace(clientIdentity.ref_contrat);
                    
                if (estSousContrat)
                {
                    /*
                        // On récupère les informations du partenaire lié à ce contrat
                        var Partenaire = await (
                            from contratpartenaire in _dbContext.ContratPartenaire
                            join partenaire in _dbContext.Partenaire on contratpartenaire.idPartenaire equals partenaire.idPartenaire
                            where contratpartenaire.referenceContrat == clientIdentity.Ref_contrat
                            select new ContratDto
                            {
                                idPartenaire = partenaire.idPartenaire,
                                nomEntreprise = partenaire.nomEntreprise,
                                referenceContrat = contratpartenaire.referenceContrat,
                                datePaiement = contratpartenaire.datePaiement
                            }).FirstOrDefaultAsync();
                    */
                    // Création d’un DTO de paiement associé à ce contrat
                    var paiement = new PaiementDto
                    {
                        clients = clientIdentity.Nom,
                        ref_contrat = clientIdentity.ref_contrat,
                        DatePaiement = DateTime.Today,
                        ModePaiement = 4,
                        EtatPaiement = 40,
                        id_etat_decompte = dernierIdEtatDecompte
                    };                    

                    // On crée un objet contenant à la fois le contrat et le paiement
                    var souscontrat = new SousContratDto
                    {
                        Paiement = paiement
                        // Contrat = Partenaire,
                    };

                    return new ApiResponse
                    {
                        Data = souscontrat,
                        Message = "Client sous contrat suivre procédure",
                        IsSuccess = true,
                        StatusCode = 200
                    };
                }
            }

            // Récupérer les données de la base de données pour le client         
            var query = await (
                from client in _dbContext.Client
                // Jointure entre Client et Prestation via id_client
                join prestation in _dbContext.Prestation on client.id_client equals prestation.id_client
                 // Jointure entre Prestation et Etat_decompte via id_prestation
                join etatDecompte in _dbContext.Etat_decompte on prestation.id_prestation equals etatDecompte.id_prestation
                // Jointure entre Prestation et Echantillon (groupée pour compter les échantillons ensuite)
                join echantillon in _dbContext.Echantillon on prestation.id_prestation equals echantillon.id_prestation into echantillonGroup
                // Jointure entre Etat_decompte et Paiement via id_etat_decompte
                join paiement in _dbContext.Paiement on etatDecompte.id_etat_decompte equals paiement.id_etat_decompte
                // Filtrage : ne prendre que les paiements ayant EtatPaiement = 21,22,23
                where (paiement.EtatPaiement == 21 || paiement.EtatPaiement == 22 || paiement.EtatPaiement == 23)  
                    // Filtrage pour s'assurer qu'on récupère les prestations du bon client
                    && client.Nom == clientIdentity.Nom
                    && client.Adresse == clientIdentity.Adresse
                    && (client.CIN == clientIdentity.Identity || client.Passport == clientIdentity.Identity)
                // On regroupe les résultats par id prestation, id etat_decompte, date et référence
                group new { etatDecompte, echantillonGroup } by new
                {
                    prestation.id_prestation,
                    etatDecompte.id_etat_decompte,
                    etatDecompte.date_etat_decompte,
                    etatDecompte.ReferenceEtatDecompte
                } into grouped
                // Projection du résultat dans un objet de type DelaiDto
                select new DelaiDto
                {     
                    id_etat_decompte = grouped.Key.id_etat_decompte,
                    datePaiement = grouped.Key.date_etat_decompte,
                    referenceEtatDecompte = grouped.Key.ReferenceEtatDecompte,
                    // On compte le nombre d'échantillons par prestation
                    nombreEchantillon = grouped.Sum(g => g.echantillonGroup.Count()) // Compter les échantillons pour chaque prestation
                }).ToListAsync();

            /*var query2 = await (
                from client in _dbContext.Client
                join prestation in _dbContext.Prestation on client.id_client equals prestation.id_client
                join etatDecompte in _dbContext.Etat_decompte on prestation.id_prestation equals etatDecompte.id_prestation
                join echantillon in _dbContext.Echantillon on prestation.id_prestation equals echantillon.id_prestation into echantillonGroup
                from echantillons in echantillonGroup.DefaultIfEmpty() // Left Join
                join paiement in _dbContext.Paiement on etatDecompte.id_etat_decompte equals paiement.id_etat_decompte into paiementGroup
                from paiements in paiementGroup.DefaultIfEmpty() // Left Join pour capturer les paiements inexistants
                where paiements == null // Filtrer les id_etat_decompte qui ne sont pas dans Paiement
                      && client.Nom == clientIdentity.Nom
                      && client.Adresse == clientIdentity.Adresse
                      && (client.CIN == clientIdentity.Identity || client.Passport == clientIdentity.Identity)
                group new { etatDecompte, echantillons } by new
                {
                    prestation.id_prestation,
                    etatDecompte.id_etat_decompte,
                    etatDecompte.date_etat_decompte,
                    etatDecompte.ReferenceEtatDecompte
                } into grouped
                select new
                {
                    id_etat_decompte = grouped.Key.id_etat_decompte,
                    DatePaiement = grouped.Key.date_etat_decompte,
                    ReferenceEtatDecompte = grouped.Key.ReferenceEtatDecompte,
                    NombreEchantillon = grouped.Sum(g => g.echantillons != null ? 1 : 0) // Compter les échantillons existants
                }).ToListAsync();*/
            
            // Deuxième requête : vérifier s’il existe un Etat_decompte sans paiement associé
            var query2 = await (
                from etatDecompte in _dbContext.Etat_decompte
                // Join avec Prestation via id_prestation
                join prestation in _dbContext.Prestation on etatDecompte.id_prestation equals prestation.id_prestation
                // Join avec Client via id_client
                join client in _dbContext.Client on prestation.id_client equals client.id_client
                // Left Join avec Paiement (pour détecter les cas où il n'y a pas encore de paiement)
                join paiement in _dbContext.Paiement on etatDecompte.id_etat_decompte equals paiement.id_etat_decompte into paiementGroup
                from paiements in paiementGroup.DefaultIfEmpty() // Left Join pour détecter les absents
                // Filtrage : ne garder que ceux qui n'ont PAS de paiement
                where paiements == null // Garde ceux qui n'ont pas de paiement
                        // Assurer que c’est bien le bon client
                        && client.Nom == clientIdentity.Nom
                        && client.Adresse == clientIdentity.Adresse
                        && (client.CIN == clientIdentity.Identity || client.Passport == clientIdentity.Identity)
                // On récupère l'id de l'etat_decompte sans paiement
                select etatDecompte.id_etat_decompte
                ).FirstOrDefaultAsync();

            // On retourne un objet ApiResponse avec les données collectées
            return new ApiResponse
            {
                Data = query, // Liste des prestations déjà payées avec infos (DelaiDto)
                ViewBag = new Dictionary<string, object>
                {
                    {"id_etat_decompte", query2} // Etat de décompte non encore payé (s’il y en a un)
                },
                Message = "vérifier",
                IsSuccess = true,
                StatusCode = 200
            };            
        }

        /*
            * Récupère les délais de paiement à valider pour les paiements en attente.
            * 
            * @returns Une tâche représentant l'opération asynchrone, contenant un ApiResponse avec les délais de paiement à valider.
        */
        public async Task<ApiResponse> GetValidationDelaiApayer()
        {
            var delai = await (
                from etat_decompte in _dbContext.Etat_decompte
                join prestation in _dbContext.Prestation on etat_decompte.id_prestation equals prestation.id_prestation
                join client in _dbContext.Client on prestation.id_client equals client.id_client
                join paiement in _dbContext.Paiement on etat_decompte.id_etat_decompte equals paiement.id_etat_decompte
                join delai_paiement in _dbContext.DelaiPaiement on paiement.idPaiement equals delai_paiement.idPaiement
                where paiement.EtatPaiement == 31 || paiement.EtatPaiement == 32 || paiement.EtatPaiement == 33
                select new DelaiDto
                {
                    id_etat_decompte = etat_decompte.id_etat_decompte,
                    nomClient = client.Nom,
                    montant = FonctionGlobalUtil.MontantReel(etat_decompte.total_montant, etat_decompte.remise),    
                    referenceEtatDecompte = etat_decompte.ReferenceEtatDecompte,
                    date_etat_decompte = etat_decompte.date_etat_decompte,
                    datePaiement = paiement.DatePaiement,
                    DateFinDelai = delai_paiement.DateFinDelai,
                    modePaiement = delai_paiement.ModePaiement
                }
            ).ToListAsync();

            return new ApiResponse
            {
                Data = delai,
                Message = delai.Any() ? "succes" : "aucun donnée trouvé",
                IsSuccess = true,
                StatusCode = 200
            };
        }

        public async Task PaiementDelaiDirect(int id_etat_decompte, int modepaiement)
        {
            var paiement = await _dbContext.Paiement
                .FirstOrDefaultAsync(p => p.id_etat_decompte == id_etat_decompte && p.ModePaiement == modepaiement);

            if (paiement != null)
            {
                paiement.DatePaiement = DateTime.Now;
                switch (modepaiement)
                {
                    case 1:
                        paiement.EtatPaiement = 11; // Paiement direct
                        break;
                    case 2:
                        paiement.EtatPaiement = 12; // Paiement par mobile
                        break;
                    /*case 3:
                        paiement.EtatPaiement = 13; // Paiement par virement
                        break;*/
                    default:
                        throw new ArgumentException("Mode de paiement invalide.");
                }
                // Logique métier pour définir l'état du paiement
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                // Gérer le cas où aucun paiement n'est trouvé
                throw new Exception($"Aucun paiement trouvé pour l'état de décompte ID {id_etat_decompte}");
            }
        }

        public async Task PaiementDelaiParChangement(int id_etat_decompte, int modepaiement)
        {
            var paiement = await _dbContext.Paiement
                .FirstOrDefaultAsync(p => p.id_etat_decompte == id_etat_decompte && p.ModePaiement == modepaiement);

            if (paiement != null)
            {
                paiement.DatePaiement = DateTime.Now;
                paiement.ModePaiement = modepaiement;
                switch (modepaiement)
                {
                    case 1:
                        paiement.EtatPaiement = 11; // Paiement direct
                        break;
                    case 2:
                        paiement.EtatPaiement = 12; // Paiement par mobile
                        break;
                    default:
                        throw new ArgumentException("Mode de paiement invalide.");
                }
                // Logique métier pour définir l'état du paiement
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                // Gérer le cas où aucun paiement n'est trouvé
                throw new Exception($"Aucun paiement trouvé pour l'état de décompte ID {id_etat_decompte}");
            }
        }

        public async Task<ApiResponse> GetDelaiEnAttente()
        {
            DateTime aujourdHui = DateTime.Today;
            //trouver le prochain lundi
            int jourAvantLundi = ((int)DayOfWeek.Monday - (int)aujourdHui.DayOfWeek + 7) % 7;
            //lundi de la prochaine semaine
            DateTime prochainLundi = aujourdHui.AddDays(jourAvantLundi); 
            //vendredi de la prochaine semaine
            DateTime prochainVendredi = prochainLundi.AddDays(4);

            Console.WriteLine($"lundi: {prochainLundi}");
            Console.WriteLine($"vendredi: {prochainVendredi}");

            var delaiSemaineProchaine = await (
                from delai in _dbContext.DelaiPaiement
                join paiement in _dbContext.Paiement on delai.idPaiement equals paiement.idPaiement
                join etat_decompte in _dbContext.Etat_decompte on paiement.id_etat_decompte equals etat_decompte.id_etat_decompte
                join prestation in _dbContext.Prestation on etat_decompte.id_prestation equals prestation.id_prestation
                where (paiement.EtatPaiement == 31 || paiement.EtatPaiement == 32)
                    && delai.DateFinDelai >= prochainLundi
                    && delai.DateFinDelai <= prochainVendredi
                group new { delai, etat_decompte } by delai.DateFinDelai.Date into g
                orderby g.Key
                select new DashboardDelaiDto
                {
                    dateDujour = g.Key,
                    nombreEnEspece = g.Count(x => x.delai.ModePaiement == 1),
                    nombreEnMobile = g.Count(x => x.delai.ModePaiement == 2),
                    montantEnEspece = g
                        .Where(x => x.delai.ModePaiement == 1)
                        .Sum(x => (double)(x.etat_decompte.total_montant) * (1 - x.etat_decompte.remise / 100.0)),
                    montantEnMobile = g
                        .Where(x => x.delai.ModePaiement == 2)
                        .Sum(x => (double)(x.etat_decompte.total_montant) * (1 - x.etat_decompte.remise / 100.0))
                }).ToListAsync();

            return new ApiResponse 
            {
                Data = delaiSemaineProchaine,
                Message = "succès",
                IsSuccess = true,
                StatusCode = 200
            };
        }

        public async Task<ApiResponse> GetPrestationApayer()
        {
            var prestations = await (
                from etat_decompte in _dbContext.Etat_decompte
                join prestation in _dbContext.Prestation on etat_decompte.id_prestation equals prestation.id_prestation
                join client in _dbContext.Client on prestation.id_client equals client.id_client
                where prestation.status_paiement == 1 && client.IsInterne == true
                orderby etat_decompte.date_etat_decompte descending
                select new PaiementDto
                {
                    id_etat_decompte = etat_decompte.id_etat_decompte,
                    clients = client.Nom,
                    montant = FonctionGlobalUtil.MontantReel(etat_decompte.total_montant, etat_decompte.remise),
                    etatDecompte = etat_decompte.ReferenceEtatDecompte,
                    DatePaiement = etat_decompte.date_etat_decompte
                }).ToListAsync();


            return new ApiResponse
            {
                Data = prestations,
                Message = "succès",
                IsSuccess = true,
                StatusCode = 200
            };
        }
        
    }
}
