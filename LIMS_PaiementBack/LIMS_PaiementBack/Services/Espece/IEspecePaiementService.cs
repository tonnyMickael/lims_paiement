using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Services
{
    public interface IEspecePaiementService
    {
        Task AddEspecePaiement(PaiementDto paiement);
        Task<ApiResponse> GetInfoEspecePaiement(int id_etat_decompte);
    }
}
