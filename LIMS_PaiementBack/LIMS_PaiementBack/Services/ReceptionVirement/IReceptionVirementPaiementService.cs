using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Services
{
    public interface IReceptionVirementPaiementService
    {
        Task<ApiResponse> GetVirementAPayer();
        Task AddVirementPaiementRecu(RecuDto recu);
        Task<ApiResponse> ListeBanqueAsync();
        Task<ApiResponse> GetVirementAConfirmer(int id_etat_decompte);
    }
}
