using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Services.EtatHebdomadaire
{
    public interface IEtatHebdomadaireService
    {
        Task SemaineAdd(SemaineDto week);
        Task<ApiResponse> AllGetSemaine();
        Task<ApiResponse> GetAllVersement();
    }
}
