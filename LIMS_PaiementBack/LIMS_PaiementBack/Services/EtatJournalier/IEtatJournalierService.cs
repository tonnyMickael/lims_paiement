using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Services.EtatJournalier
{
    public interface IEtatJournalierService
    {
        Task<ApiResponse> GetAllEtatJournalier();
    }
}
