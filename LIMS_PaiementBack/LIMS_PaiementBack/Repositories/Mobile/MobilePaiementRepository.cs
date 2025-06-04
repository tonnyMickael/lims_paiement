using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Utils;
using Microsoft.EntityFrameworkCore;

namespace LIMS_PaiementBack.Repositories
{
    public class MobilePaiementRepository : IMobilePaiementRepository
    {
        private readonly DbContextEntity _dbContext;

        public MobilePaiementRepository(DbContextEntity dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddPaiementMobile(PaiementEntity paiement)
        {
            // Mettre les opérations en transaction pour garantir l'intégrité
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    await _dbContext.Paiement.AddAsync(paiement);
                    await _dbContext.SaveChangesAsync();

                    await _dbContext.Etat_decompte
                        .Where(e => e.id_etat_decompte == paiement.id_etat_decompte)
                        .ExecuteUpdateAsync(setters => setters.SetProperty(e => e.date_paiement, paiement.DatePaiement));

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<ApiResponse> GetDataPaiementMobile(int id_etat_decompte)
        {
            var information = await (
                from client in _dbContext.Client
                join prestation in _dbContext.Prestation on client.id_client equals prestation.id_client
                join etatDecompte in _dbContext.Etat_decompte on prestation.id_prestation equals etatDecompte.id_prestation
                where etatDecompte.id_etat_decompte == id_etat_decompte
                select new PaiementDto
                {
                    titre = "Par mobile",
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
    }
}
