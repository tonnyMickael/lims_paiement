using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Repositories.EtatJournalier
{
    public interface IEtatJournalierRepository
    {
        Task<ApiResponse> GetEtatJournalier();
    }
}
