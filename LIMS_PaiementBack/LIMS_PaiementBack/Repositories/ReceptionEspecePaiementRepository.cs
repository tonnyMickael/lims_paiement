using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace LIMS_PaiementBack.Repositories
{
    public class ReceptionEspecePaiementRepository : IReceptionEspecePaiementRepository
    {
        private readonly DbContextEntity _dbContext;

        public ReceptionEspecePaiementRepository(DbContextEntity dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddRecuEspecePaiement(ReceptionEspeceEntity recu)
        {
            await _dbContext.ReceptionEspece.AddAsync(recu);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ApiResponse> GetDataEspeceAPayer()
        {
            var idEspecePaiement = await _dbContext.Paiement
                .Where(p => p.ModePaiement == 1)
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
