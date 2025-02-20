using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Services
{
    public interface IRefusPaiementService
    {
        Task<ApiResponse> GetAllListRefus();
        Task AddRefusPaiementAsync(RefusDto refus);
    }
}
