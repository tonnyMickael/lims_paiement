using LIMS_PaiementBack.Utils;
using LIMS_PaiementBack.Entities;

namespace LIMS_PaiementBack.Repositories
{
    public interface IDelaiRepository
    {
        Task AddDelaiPaiement(DelaiEntity delai, PaiementEntity paiement);
        Task<ApiResponse> GetAllDelaiAsync();
        Task<ApiResponse> GetValidationDelai(int id_etat_decompte);
        Task<ApiResponse> GetValidationDelaiApayer();
        Task PaiementDelaiDirect(int id_etat_decompte, int modepaiement);
        Task PaiementDelaiParChangement(int id_etat_decompte, int modepaiement);
        Task<ApiResponse> GetDelaiEnAttente();
        Task<ApiResponse> GetPrestationApayer();
    }
}