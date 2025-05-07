using LIMS_PaiementBack.Utils;
using LIMS_PaiementBack.Models;

namespace LIMS_PaiementBack.Services
{
    public interface IDelaiService
    {
        Task AddDelaiAsync(DelaiDto delai);
        Task<ApiResponse> GetDelaiPaiement();
        Task<ApiResponse> GetDelaiAccorder(int id_etat_decompte);
        Task<ApiResponse> GetDelaiApayer();
        Task PaiementDelaiDirectAsync(int id_etat_decompte, int modepaiement);
        Task PaiementDelaiParChangementAsync(int id_etat_decompte, int modepaiement);
        Task<ApiResponse> GetDelaiEnAttenteAsync();
        Task<ApiResponse> GetPrestationApayerAsync();
    }
}
