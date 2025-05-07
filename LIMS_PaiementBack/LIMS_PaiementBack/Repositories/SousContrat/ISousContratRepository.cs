using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Repositories
{
    public interface ISousContratRepository
    {
        public Task<ApiResponse> GetListeSousContratPaiement();
        public Task<ApiResponse> GetListeSousContratApayer();
        // public Task AddSousContrat(PaiementEntity paiement, int id_partenaire);
        public Task AddSousContrat(PaiementEntity paiement);
        public Task UpdateEtatPaiementSousContrat(int id_etat_decompte);
    }
}
