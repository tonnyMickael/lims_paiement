using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Repositories.EtatHebdomadaire
{
    public interface IEtatHebdomadaireRepository
    {
        Task AddSemaine(SemaineEntity weeks);
        Task<ApiResponse> GetAllSemaine();
        Task<ApiResponse> GetAllVersementHebdomadaire();
    }
}
