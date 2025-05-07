using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Repositories;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Services
{
    public class ReceptionMobilePaiementService : IReceptionMobilePaiementService
    {
        private readonly IReceptionMobilePaiementRepository _receptionMobilePaiementRepository;
        //private readonly Email _email;

        public ReceptionMobilePaiementService(IReceptionMobilePaiementRepository receptionMobilePaiementRepository)
        {
            _receptionMobilePaiementRepository = receptionMobilePaiementRepository;
            //_email = email;
        }

        public async Task AddMobilePaiementRecu(RecuDto recu)
        {
            var recepeiton = new ReceptionMobileEntity
            {
                Reference = recu.referenceMobile,
                operateurmobile = recu.operateurmobile,
                idPaiement = recu.id_paiement
            };

            await _receptionMobilePaiementRepository.AddRecuMobilePaiement(recepeiton);
            // Envoyer un email si l'adresse email est renseignée
            /*if (!string.IsNullOrEmpty(paiement.email))
            {
                await _email.SendEmailAsync(paiement.email, "Confirmation de paiement",
                    $"Votre paiement de {paiement.montant} a été reçu avec succès.");
            }*/
        }

        public async Task<ApiResponse> GetMobileAPayer()
        {
            return await _receptionMobilePaiementRepository.GetDataMobileAPayer();
        }

        public async Task<ApiResponse> GetMobileAConfirmer()
        {
            return await _receptionMobilePaiementRepository.GetMobileAConfirmerPayer();
        }
    }
}
