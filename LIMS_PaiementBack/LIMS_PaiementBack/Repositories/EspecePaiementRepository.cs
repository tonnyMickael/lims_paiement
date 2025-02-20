using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Utils;
using Microsoft.EntityFrameworkCore;

namespace LIMS_PaiementBack.Repositories
{
    public class EspecePaiementRepository : IEspecePaiementRepository
    {
        private readonly DbContextEntity _dbContext;

        public EspecePaiementRepository(DbContextEntity dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddPaiementEspece(PaiementEntity paiement)
        {
            await _dbContext.Paiement.AddAsync(paiement);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ApiResponse> GetDataPaiementEspece(int id_etat_decompte)
        {
            var information = await (
                from client in _dbContext.Client
                join prestation in _dbContext.Prestation on client.id_client equals prestation.id_client
                join etatDecompte in _dbContext.Etat_decompte on prestation.id_prestation equals etatDecompte.id_prestation
                where etatDecompte.id_etat_decompte == id_etat_decompte
                select new PaiementDto
                {
                    titre = "En espece",
                    clients = client.Nom,
                    email = client.Email,
                    adresse = client.Adresse,
                    contact = client.Contact,
                    identite = FonctionGlobalUtil.GetClientIdentity(client.CIN, client.Passport),
                    montant = FonctionGlobalUtil.MontantReel(prestation.total_montant, prestation.remise),
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
    }
}
