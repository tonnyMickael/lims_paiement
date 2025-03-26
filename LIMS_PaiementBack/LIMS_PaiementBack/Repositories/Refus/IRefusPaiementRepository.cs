using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Repositories
{
    public interface IRefusPaiementRepository
    {
        public Task<ApiResponse> GetListRefusPaiement();
        public Task AddRefusPaiement(PaiementEntity paiement, RefusEntity refuser);
    }
}
