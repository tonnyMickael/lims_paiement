using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Utils;
using Microsoft.EntityFrameworkCore;

namespace LIMS_PaiementBack.Repositories
{
    public class ReceptionMobilePaiementRepository : IReceptionMobilePaiementRepository 
    {
        private readonly DbContextEntity _dbContext;

        public ReceptionMobilePaiementRepository(DbContextEntity dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddRecuMobilePaiement(ReceptionMobileEntity recu)
        {
            await _dbContext.ReceptionMobile.AddAsync(recu);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ApiResponse> GetDataMobileAPayer()
        {
            var idEspecePaiement = await _dbContext.Paiement
                .Where(p => p.ModePaiement == 2)
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
