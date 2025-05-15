using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Repositories
{
    public interface IReceptionMobilePaiementRepository
    {
        Task AddRecuMobilePaiement(ReceptionMobileEntity recu);
        Task<ApiResponse> GetDataMobileAPayer();
        Task<ApiResponse> GetMobileAConfirmerPayer();
        Task<ApiResponse> GetMobileOperateur();
    }
}
