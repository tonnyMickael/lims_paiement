using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Repositories;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Services
{
    public class RefusPaiementService : IRefusPaiementService
    {
        private readonly IRefusPaiementRepository _refusPaiementRepository;

        public RefusPaiementService(IRefusPaiementRepository refundPaiementRepository)
        {
            _refusPaiementRepository = refundPaiementRepository;
        }

        public async Task AddRefusPaiementAsync(RefusDto refus)
        {
            var paiement = new PaiementEntity
            {
                DatePaiement = refus.DatePaiement,
                ModePaiement = 4,
                EtatPaiement = 404,
                id_etat_decompte = refus.id_etat_decompte
            };

            var refuser = new RefusEntity
            {
                motifs = refus.motifs
            };

            await _refusPaiementRepository.AddRefusPaiement(paiement, refuser);
        }

        public async Task<ApiResponse> GetAllListRefus()
        {
            return await _refusPaiementRepository.GetListRefusPaiement();
        }
    }
}
