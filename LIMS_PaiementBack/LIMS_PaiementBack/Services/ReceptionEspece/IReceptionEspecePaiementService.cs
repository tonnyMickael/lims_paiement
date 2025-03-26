using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Services
{
    public interface IReceptionEspecePaiementService
    {
        Task<ApiResponse> GetEspeceAPayer();
        Task AddEspecePaiementRecu(RecuDto recu);
    }
}
