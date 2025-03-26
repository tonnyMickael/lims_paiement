using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Utils;
using Microsoft.EntityFrameworkCore;

namespace LIMS_PaiementBack.Repositories
{
    public class RefusPaiementRepository : IRefusPaiementRepository
    {
        private readonly DbContextEntity _dbContext;
        
        public RefusPaiementRepository(DbContextEntity dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddRefusPaiement(PaiementEntity paiement, RefusEntity refuser)
        {
            await _dbContext.Paiement.AddAsync(paiement);
            await _dbContext.SaveChangesAsync();

            refuser.idPaiement = paiement.idPaiement;

            await _dbContext.Refus.AddAsync(refuser);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ApiResponse> GetListRefusPaiement()
        {
            var listRefus = await (
                from refus in _dbContext.Refus
                join paiement in _dbContext.Paiement on refus.idPaiement equals paiement.idPaiement
                join etat_decompte in _dbContext.Etat_decompte on paiement.id_etat_decompte equals etat_decompte.id_etat_decompte
                select new RefusDto
                {
                    id_paiement = paiement.idPaiement,
                    motifs = refus.motifs,
                    DatePaiement = paiement.DatePaiement ?? default(DateTime),
                    EtatPaiement = paiement.EtatPaiement,
                    referenceEtatDecompte = etat_decompte.ReferenceEtatDecompte
                }).ToListAsync();


            return new ApiResponse
            {
                Data = listRefus,
                Message = listRefus.Any() ? "succces" : "donnée irrécupérable",
                IsSuccess = true,
                StatusCode = 200
            };
        }
    }
}
