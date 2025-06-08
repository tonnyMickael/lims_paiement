using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Repositories
{
    public interface IBanqueRepository
    {
        Task AddNewBanque(BanqueEntity banque);
        Task<ApiResponse> AllBanque(int position, int pagesize);
        Task<ApiResponse> GetModificationBanque(int id_banque);
        Task ModificationBanque(int Id_banque, BanqueDto banque);
    }
}