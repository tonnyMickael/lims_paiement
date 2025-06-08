using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Services
{
    public interface IBanqueService
    {
        Task AddNewBanqueAsync(BanqueDto banque);
        Task<ApiResponse> AllBanqueAsync(int position, int pagesize);
        Task<ApiResponse> GetModificationBanqueAsync(int id_banque);
        Task ModificationBanqueAsync(int Id_banque, BanqueDto banque);
    }
}