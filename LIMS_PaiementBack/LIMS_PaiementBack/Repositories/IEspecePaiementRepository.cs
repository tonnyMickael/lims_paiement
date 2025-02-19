using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Repositories
{
    public interface IEspecePaiementRepository
    {
        Task AddPaiementEspece(PaiementEntity paiement);
        Task<ApiResponse> GetDataPaiementEspece(int id_etat_decompte);
    }
}
