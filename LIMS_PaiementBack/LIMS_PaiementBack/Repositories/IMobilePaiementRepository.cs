using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Repositories
{
    public interface IMobilePaiementRepository
    {
        Task AddPaiementMobile(PaiementEntity paiement);
        Task<ApiResponse> GetDataPaiementMobile(int id_etat_decompte);
    }
}
