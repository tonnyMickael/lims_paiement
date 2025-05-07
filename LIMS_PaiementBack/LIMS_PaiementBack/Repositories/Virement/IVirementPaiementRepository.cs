using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Repositories
{
    public interface IVirementPaiementRepository
    {
        Task AddPaiementVirement(PaiementEntity paiement);
        Task<ApiResponse> GetDataPaiementVirement(int id_etat_decompte);
        Task<ApiResponse> GetListeVirementApayer();
    }
}
