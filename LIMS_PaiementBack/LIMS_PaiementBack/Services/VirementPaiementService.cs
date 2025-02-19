using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Repositories;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Services
{
    public class VirementPaiementService : IVirementPaiementService
    {
        private readonly IVirementPaiementRepository _virementPaiementRepository;

        public VirementPaiementService(IVirementPaiementRepository virementPaiementRepository)
        {
            _virementPaiementRepository = virementPaiementRepository;
        }

        public async Task AddVirementPaiement(PaiementDto paiement)
        {
            var paiements = new PaiementEntity
            {
                DatePaiement = paiement.DatePaiement,
                ModePaiement = 3,
                EtatPaiement = 13,
                id_etat_decompte = paiement.id_etat_decompte
            };

            await _virementPaiementRepository.AddPaiementVirement(paiements);
        }

        public async Task<ApiResponse> GetInfoVirementPaiement(int id_etat_decompte)
        {
            return await _virementPaiementRepository.GetDataPaiementVirement(id_etat_decompte);
        }
    }
}
