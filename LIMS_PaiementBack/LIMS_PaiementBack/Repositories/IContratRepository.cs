using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Repositories
{
    public interface IContratRepository
    {
        Task AddNewContrat(PartenaireEntity partenaire, ContratPartenaireEntity contrat);
        Task<ApiResponse> AllContratPartenaire();
        Task ModificationEtatContrat(int id_partenaire, int id_contrat, ContratDto contrat);
        Task<ApiResponse> Contrat_a_modifier(int id_partenaire);
    }
}
