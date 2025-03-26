using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Repositories
{
    public interface IReceptionEspecePaiementRepository
    {
        Task AddRecuEspecePaiement(ReceptionEspeceEntity recu);
        Task<ApiResponse> GetDataEspeceAPayer();
    }
}
