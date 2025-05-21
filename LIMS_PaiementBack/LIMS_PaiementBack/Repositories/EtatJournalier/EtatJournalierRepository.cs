using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace LIMS_PaiementBack.Repositories.EtatJournalier
{
    public class EtatJournalierRepository : IEtatJournalierRepository
    {

        private readonly DbContextEntity _dbContext;

        public EtatJournalierRepository(DbContextEntity dbContext)
        {
            _dbContext = dbContext;
        }

        /*
            * Cette méthode récupère les états de compte journalier pour la date actuelle.
            * Elle effectue plusieurs jointures entre les tables Etat_decompte, Prestation, Client et Paiement
            * pour obtenir les informations nécessaires.
            * 
            * Elle vérifie également si l'état de compte existe déjà dans la table EtatJournalier avant de l'ajouter.
            * 
            * Enfin, elle retourne une liste d'objets EncaissementJournalierDto contenant les détails des encaissements.
        */
        public async Task<ApiResponse> GetEtatJournalier()
        {
            try
            {
                var today = DateTime.Today;
                //var today = new DateTime(2025, 01, 20);

                //valeur de paiement confirmer pour espece, mobile, virement
                // var etatsVoulus = new[] { 21, 22, 23 };
                var etatsVoulus = new[] { 1, 2, 3 };

                /*
                    * Récupérer les états de compte pour la date actuelle
                    * Effectuer des jointures entre les tables Etat_decompte, Prestation, Client et Paiement
                    * Filtrer les résultats en fonction de la date de paiement et de l'état de paiement
                    * Vérifier si l'état de compte existe déjà dans la table EtatJournalier
                */
                var result = await _dbContext.Etat_decompte
                    .Where(etat => EF.Functions.DateDiffDay(etat.date_etat_decompte, today) == 0)
                    .Join(
                        _dbContext.Prestation,
                        etat => etat.id_prestation,  // Clé étrangère vers Prestation
                        prestation => prestation.id_prestation,
                        (etat, prestation) => new { Etat = etat, Prestation = prestation }
                    )
                    .Join(
                        _dbContext.Client,
                        etatPrestation => etatPrestation.Prestation.id_client, // Clé étrangère vers Client
                        client => client.id_client,
                        (etatPrestation, client) => new { etatPrestation.Etat, Client = client }
                    )
                    .Join(
                        _dbContext.Paiement,
                        etatClient => etatClient.Etat.id_etat_decompte,
                        paiement => paiement.id_etat_decompte,
                        (etatClient, paiement) => new { etatClient.Etat, etatClient.Client, Paiement = paiement }
                    )
                    .Where(joined =>
                        joined.Paiement != null && EF.Functions.DateDiffDay(joined.Paiement.DatePaiement, today) == 0
                        && etatsVoulus.Contains(joined.Paiement.ModePaiement)
                        && joined.Paiement.EtatPaiement == true
                    )
                    .ToListAsync();

                // Charger tous les id_etat_decompte déjà existants
                var idsExistants = await _dbContext.EtatJournalier
                    .Select(e => e.id_etat_decompte)
                    .ToListAsync();

                var nouveauxEtatsJournalier = new List<EtatJournalierEntity>();

                foreach (var item in result)
                {
                    if (!idsExistants.Contains(item.Etat.id_etat_decompte)) // Vérification rapide en mémoire
                    {
                        nouveauxEtatsJournalier.Add(new EtatJournalierEntity
                        {
                            DateEncaissement = today,
                            Observation = item.Client.IsInterne ? 2 : 1, // Si client.isinterne = true → 2, sinon → 1
                            id_etat_decompte = item.Etat.id_etat_decompte
                        });
                    }
                }

                // Ajouter tous les nouveaux enregistrements en une seule fois
                if (nouveauxEtatsJournalier.Any())
                {
                    await _dbContext.EtatJournalier.AddRangeAsync(nouveauxEtatsJournalier);
                    await _dbContext.SaveChangesAsync();
                }                
                
                /*
                    * Récupérer les états de compte journalier pour la date actuelle
                    * Effectuer des jointures entre les tables Etat_decompte, Prestation, Client et Paiement
                    * Filtrer les résultats en fonction de la date d'encaissement et de l'état de paiement
                    * Retourner une liste d'objets EncaissementJournalierDto contenant les détails des encaissements
                */
                var listeJournalier = await (
                    from etat_journalier in _dbContext.EtatJournalier
                    join etat_Decompte in _dbContext.Etat_decompte on etat_journalier.id_etat_decompte equals etat_Decompte.id_etat_decompte
                    join paiement in _dbContext.Paiement on etat_Decompte.id_etat_decompte equals paiement.id_etat_decompte
                    join prestation in _dbContext.Prestation on etat_Decompte.id_prestation equals prestation.id_prestation
                    join client in _dbContext.Client on prestation.id_client equals client.id_client
                    where etatsVoulus.Contains(paiement.ModePaiement) 
                        && paiement.EtatPaiement == true
                        && etat_journalier.DateEncaissement == today
                    select new EncaissementJournalierDto {
                        dateEncaissement = etat_journalier.DateEncaissement,
                        EtatDecompte = etat_Decompte.ReferenceEtatDecompte,
                        clients = client.Nom,
                        montant = FonctionGlobalUtil.MontantReel(etat_Decompte.total_montant, etat_Decompte.remise),
                        observation = etat_journalier.Observation
                    }).ToListAsync();

                return new ApiResponse
                {
                    Data = listeJournalier,
                    Message = listeJournalier.Any() ? "succes" : "erreur", 
                    IsSuccess = true,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Data = null,
                    Message = $"Exeption:{ex.Message}",
                    IsSuccess = false,
                    StatusCode = 400
                };
            }
        }
    }
}
