using LIMS_PaiementBack.Repositories.Depart;
using LIMS_PaiementBack.Repositories.EtatJournalier;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Services.EtatJournalier
{
    public class EtatJournalierService : IEtatJournalierService
    {
        private readonly IEtatJournalierRepository _journalierRepository;

        public EtatJournalierService(IEtatJournalierRepository journalierRepository)
        {
            _journalierRepository = journalierRepository;
        }

        public async Task<ApiResponse> GetAllEtatJournalier()
        {
            return await _journalierRepository.GetEtatJournalier();
        }
    }
}
