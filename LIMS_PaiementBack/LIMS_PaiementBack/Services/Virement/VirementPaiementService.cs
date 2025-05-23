using LIMS_PaiementBack.Utils;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Repositories;

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
                id_modePaiement = paiement.id_modePaiement,
                EtatPaiement = false,
                nomDuPayant = paiement.nomPayant,
                prenomDuPayant = paiement.prenomPayant,
                contactdupayant = paiement.contactdupayant,
                id_etat_decompte = paiement.id_etat_decompte
            };

            await _virementPaiementRepository.AddPaiementVirement(paiements);
        }

        public async Task<ApiResponse> GetInfoVirementPaiement(int id_etat_decompte)
        {
            return await _virementPaiementRepository.GetDataPaiementVirement(id_etat_decompte);
        }

        public async Task<ApiResponse> GetListeVirementApayerAsync()
        {
            return await _virementPaiementRepository.GetListeVirementApayer();
        }
    }
}
