using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks.Dataflow;

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
        }

        //afficher tout les delai qui ont été accorder
        public async Task<ApiResponse> GetAllDelaiAsync()
        {
            var delaiList = await (from etat_decompte in _dbContext.Etat_decompte
                                  join paiement in _dbContext.Paiement
                                    on etat_decompte.id_etat_decompte equals paiement.id_etat_decompte
                                  join delai_paiement in _dbContext.DelaiPaiement
                                    on paiement.idPaiement equals delai_paiement.idPaiement    
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
        public async Task<ApiResponse> GetValidationDelai(int id_etat_decompte)
        {     
            var dernierIdEtatDecompte = id_etat_decompte;

            //recherche du client via l'id_etat_decompte
            var clientIdentity = await (
                from client in _dbContext.Client
                join prestation in _dbContext.Prestation on client.id_client equals prestation.id_client
                join etatDecompte in _dbContext.Etat_decompte on prestation.id_prestation equals etatDecompte.id_prestation
                where etatDecompte.id_etat_decompte == dernierIdEtatDecompte
                select new
                {
                    Nom = client.Nom,
                    Adresse = client.Adresse,
                    Identity = FonctionGlobalUtil.GetClientIdentity(client.CIN, client.Passport),
                    Ref_contrat = client.ref_contrat
                }).FirstOrDefaultAsync();


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
                bool estSousContrat = await _dbContext.ContratPartenaire
                    .AnyAsync(e => e.referenceContrat == clientIdentity.Ref_contrat);
                if (estSousContrat)
                {
                    return new ApiResponse
                    {
                        Data = dernierIdEtatDecompte,
                        Message = "Client sous contrat suivre procédure",
                        IsSuccess = true,
                        StatusCode = 200
                    };
                }
            }

            // Récupérer les données de la base de données pour le client         
            var query = await (
                 from client in _dbContext.Client
                 join prestation in _dbContext.Prestation on client.id_client equals prestation.id_client
                 join etatDecompte in _dbContext.Etat_decompte on prestation.id_prestation equals etatDecompte.id_prestation
                 join echantillon in _dbContext.Echantillon on prestation.id_prestation equals echantillon.id_prestation into echantillonGroup
                 join paiement in _dbContext.Paiement on etatDecompte.id_etat_decompte equals paiement.id_etat_decompte
                 where paiement.EtatPaiement == 11 
                       && client.Nom == clientIdentity.Nom
                       && client.Adresse == clientIdentity.Adresse
                       && (client.CIN == clientIdentity.Identity || client.Passport == clientIdentity.Identity)
                 group new { etatDecompte, echantillonGroup } by new
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
                     NombreEchantillon = grouped.Sum(g => g.echantillonGroup.Count()) // Compter les échantillons pour chaque prestation
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

            var query2 = await (
                from etatDecompte in _dbContext.Etat_decompte
                join prestation in _dbContext.Prestation on etatDecompte.id_prestation equals prestation.id_prestation
                join client in _dbContext.Client on prestation.id_client equals client.id_client
                join paiement in _dbContext.Paiement on etatDecompte.id_etat_decompte equals paiement.id_etat_decompte into paiementGroup
                from paiements in paiementGroup.DefaultIfEmpty() // Left Join pour détecter les absents
                where paiements == null // Garde ceux qui n'ont pas de paiement
                        && client.Nom == clientIdentity.Nom
                        && client.Adresse == clientIdentity.Adresse
                        && (client.CIN == clientIdentity.Identity || client.Passport == clientIdentity.Identity)
                select etatDecompte.id_etat_decompte
                ).FirstOrDefaultAsync();

            return new ApiResponse
            {
                Data = query,
                ViewBag = new Dictionary<string, object>
                {
                    {"id_etat_decompte", query2}
                },
                Message = "vérifier",
                IsSuccess = true,
                StatusCode = 200
            };            
        }

    }
}
