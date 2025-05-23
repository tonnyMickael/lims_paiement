using LIMS_PaiementBack.Utils;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Entities;
using Microsoft.EntityFrameworkCore;

namespace LIMS_PaiementBack.Repositories
{
    public class VirementPaiementRepository : IVirementPaiementRepository
    {
        private readonly DbContextEntity _dbContext;

        public VirementPaiementRepository(DbContextEntity dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddPaiementVirement(PaiementEntity paiement)
        {
            await _dbContext.Paiement.AddAsync(paiement);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ApiResponse> GetDataPaiementVirement(int id_etat_decompte)
        {
            var information = await (
                from client in _dbContext.Client
                join prestation in _dbContext.Prestation on client.id_client equals prestation.id_client
                join etatDecompte in _dbContext.Etat_decompte on prestation.id_prestation equals etatDecompte.id_prestation
                where etatDecompte.id_etat_decompte == id_etat_decompte
                select new PaiementDto
                {
                    titre = "Par virement",
                    clients = client.Nom,
                    email = client.Email,
                    adresse = client.Adresse,
                    contact = client.Contact,
                    identite = FonctionGlobalUtil.GetClientIdentity(client.CIN ?? "", client.Passport ?? ""),
                    montant = FonctionGlobalUtil.MontantReel(etatDecompte.total_montant, etatDecompte.remise),
                    etatDecompte = etatDecompte.ReferenceEtatDecompte,
                    id_etat_decompte = id_etat_decompte
                }).ToListAsync();

            return new ApiResponse
            {
                Data = information,
                Message = "succes",
                IsSuccess = true,
                StatusCode = 200
            };
        }

        public async Task<ApiResponse> GetListeVirementApayer()
        {
            var virementListe = await (
                from client in _dbContext.Client
                join prestation in _dbContext.Prestation on client.id_client equals prestation.id_client
                join etat_decompte in _dbContext.Etat_decompte on prestation.id_prestation equals etat_decompte.id_prestation
                join paiement in _dbContext.Paiement on etat_decompte.id_etat_decompte equals paiement.id_etat_decompte
                where paiement.id_modePaiement == 3 && paiement.EtatPaiement == true
                orderby paiement.idPaiement descending
                select new PaiementDto
                {
                    id_etat_decompte = etat_decompte.id_etat_decompte,
                    clients = client.Nom,
                    montant = FonctionGlobalUtil.MontantReel(etat_decompte.total_montant, etat_decompte.remise),
                    DatePaiement = paiement.DatePaiement ?? default(DateTime),
                    etatDecompte = etat_decompte.ReferenceEtatDecompte
                } 
            ).ToListAsync();

            return new ApiResponse {
                Data = virementListe,
                Message = virementListe.Any() ? "succès" : "échec",
                IsSuccess = true,
                StatusCode = 200
            };
        }
    }
}
