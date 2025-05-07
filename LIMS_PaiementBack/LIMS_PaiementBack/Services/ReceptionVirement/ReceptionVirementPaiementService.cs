using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Repositories;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Services
{
    public class ReceptionVirementPaiementService : IReceptionVirementPaiementService
    {
        private readonly IReceptionVirementPaiementRepository _receptionVirementPaiementRepository;
        //private readonly Email _email;

        public ReceptionVirementPaiementService(IReceptionVirementPaiementRepository receptionVirementPaiementRepository)
        {
            _receptionVirementPaiementRepository = receptionVirementPaiementRepository;
            //_email = email;
        }

        public async Task AddVirementPaiementRecu(RecuDto recu)
        {
            var recepeiton = new OrdreDeVirementEntity
            {
                reference = recu.referenceOV,
                idPaiement = recu.id_paiement
            };

            await _receptionVirementPaiementRepository.AddRecuVirementPaiement(recepeiton);

            // Envoyer un email si l'adresse email est renseignée
            /*if (!string.IsNullOrEmpty(paiement.email))
            {
                await _email.SendEmailAsync(paiement.email, "Confirmation de paiement",
                    $"Votre paiement de {paiement.montant} a été reçu avec succès.");
            }*/
        }

        public async Task<ApiResponse> GetVirementAPayer()
        {
            return await _receptionVirementPaiementRepository.GetDataVirementAPayer();
        }

        public async Task<ApiResponse> GetVirementAConfirmer(int id_etat_decompte)
        {
            return await _receptionVirementPaiementRepository.GetVirementAConfirmerPayer(id_etat_decompte);
        }
    }
}
