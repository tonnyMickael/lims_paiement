using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Repositories
{
    public interface IDelaiRepository
    {
        Task AddDelaiPaiement(DelaiEntity delai, PaiementEntity paiement);
        Task<ApiResponse> GetAllDelaiAsync();
        Task<ApiResponse> GetValidationDelai(int id_etat_decompte);
        //Task GetValidationDelai(int id_etat_decompte);
    }
}
