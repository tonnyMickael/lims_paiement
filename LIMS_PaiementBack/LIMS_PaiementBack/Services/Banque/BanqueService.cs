using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Repositories;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Services
{
    public class BanqueService : IBanqueService
    {
        private readonly IBanqueRepository _banqueRepository;

        public BanqueService(IBanqueRepository banqueRepository)
        {
            _banqueRepository = banqueRepository;
        }
        
        public async Task AddNewBanqueAsync(BanqueDto banque)
        {
            var banqueEntity = new BanqueEntity
            {
                designation = banque.designation
            };

            await _banqueRepository.AddNewBanque(banqueEntity);
        }

        public async Task<ApiResponse> AllBanqueAsync(int position, int pagesize)
        {
            return await _banqueRepository.AllBanque(position, pagesize);
        }

        public async Task<ApiResponse> GetModificationBanqueAsync(int id_banque)
        {
            return await _banqueRepository.GetModificationBanque(id_banque);
        }

        public async Task ModificationBanqueAsync(int Id_banque, BanqueDto banque)
        {
            await _banqueRepository.ModificationBanque(Id_banque, banque);
        }
        
    }
}