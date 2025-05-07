using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Services
{
    public interface ISousContratService
    {
        Task<ApiResponse> GetAllListeSousContrat();
        Task<ApiResponse> GetListeSousContratApayerAsync();
        Task AddSousContratAsync(SousContratDto contrat);
        Task UpdateEtatPaiementSousContrat(int id_etat_decompte);
    }
}