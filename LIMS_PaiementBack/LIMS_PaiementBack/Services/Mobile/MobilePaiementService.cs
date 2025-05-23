using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Repositories;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Services
{
    public class MobilePaiementService : IMobilePaiementService
    {
        private readonly IMobilePaiementRepository _mobilePaiementRepository;

        public MobilePaiementService(IMobilePaiementRepository mobilePaiementRepository)
        {
            _mobilePaiementRepository = mobilePaiementRepository;
        }       

        public async Task AddMobilePaiement(PaiementDto paiement)
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

            await _mobilePaiementRepository.AddPaiementMobile(paiements);
        }

        public async Task<ApiResponse> GetInfoMobilePaiement(int id_etat_decompte)
        {
            return await _mobilePaiementRepository.GetDataPaiementMobile(id_etat_decompte);
        }
    }
}
