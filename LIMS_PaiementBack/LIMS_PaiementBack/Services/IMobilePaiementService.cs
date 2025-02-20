using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Services
{
    public interface IMobilePaiementService
    {
        Task AddMobilePaiement(PaiementDto paiement);
        Task<ApiResponse> GetInfoMobilePaiement(int id_etat_decompte);
    }
}
