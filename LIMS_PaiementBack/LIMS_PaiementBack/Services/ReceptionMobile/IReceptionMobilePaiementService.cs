using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Services
{
    public interface IReceptionMobilePaiementService
    {
        Task<ApiResponse> GetMobileAPayer();
        Task AddMobilePaiementRecu(RecuDto recu);
        Task<ApiResponse> GetMobileAConfirmer();
        Task<ApiResponse> GetMobileOperateurAsync();
    }
}
