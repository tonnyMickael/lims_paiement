using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Services
{
    public interface IVirementPaiementService
    {
        Task AddVirementPaiement(PaiementDto paiement);
        Task<ApiResponse> GetInfoVirementPaiement(int id_etat_decompte);
    }
}
