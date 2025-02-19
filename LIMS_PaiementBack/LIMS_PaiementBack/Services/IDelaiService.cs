using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Services
{
    public interface IDelaiService
    {
        Task AddDelaiAsync(DelaiDto delai);
        Task<ApiResponse> GetDelaiPaiement();
        Task<ApiResponse> GetDelaiAccorder(int id_etat_decompte);
    }
}
