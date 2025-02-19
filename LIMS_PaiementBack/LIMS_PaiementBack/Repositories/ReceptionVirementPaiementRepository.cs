using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Utils;
using Microsoft.EntityFrameworkCore;

namespace LIMS_PaiementBack.Repositories
{
    public class ReceptionVirementPaiementRepository : IReceptionVirementPaiementRepository
    {
        private readonly DbContextEntity _dbContext;

        public ReceptionVirementPaiementRepository(DbContextEntity dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddRecuVirementPaiement(OrdreDeVirementEntity recu)
        {
            await _dbContext.OrdreVirement.AddAsync(recu);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ApiResponse> GetDataVirementAPayer()
        {
            var idEspecePaiement = await _dbContext.Paiement
                .Where(p => p.ModePaiement == 3)
                .OrderByDescending(p => p.idPaiement)
                .Select(p => (int?)p.idPaiement)
                .FirstOrDefaultAsync();

            return new ApiResponse
            {
                Data = idEspecePaiement,
                Message = "succes",
                IsSuccess = true,
                StatusCode = 200
            };
        }
    }
}
