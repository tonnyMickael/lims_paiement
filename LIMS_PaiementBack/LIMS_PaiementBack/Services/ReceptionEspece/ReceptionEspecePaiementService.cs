using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Repositories;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Services
{
    public class ReceptionEspecePaiementService : IReceptionEspecePaiementService
    {
        private readonly IReceptionEspecePaiementRepository _receptionEspecePaiementRepository;
        //private readonly Email _email;
        //private readonly Sms _smsService;

        public ReceptionEspecePaiementService(IReceptionEspecePaiementRepository receptionEspecePaiementRepository)
        {
            _receptionEspecePaiementRepository = receptionEspecePaiementRepository;
        }

        public async Task AddEspecePaiementRecu(RecuDto recu)
        {
            var recepeiton = new ReceptionEspeceEntity
            {
                montantRecu = recu.montantRecu,
                idPaiement = recu.id_paiement
            };

            await _receptionEspecePaiementRepository.AddRecuEspecePaiement(recepeiton);

            // Envoyer un email si l'adresse email est renseignée
            /*if (!string.IsNullOrEmpty(paiement.email))
            {
                await _email.SendEmailAsync(paiement.email, "Confirmation de paiement",
                    $"Votre paiement de {paiement.montant} a été reçu avec succès.");
            }*/          

            // Envoi de la notification Firebase
            /*var notificationService = new Sms();
            await notificationService.SendNotificationAsync(recu.firebaseToken, "Paiement Reçu", $"Votre paiement de {recu.montantRecu} MGA a été validé.");*/
        }

        public async Task<ApiResponse> GetEspeceAPayer()
        {
            return await _receptionEspecePaiementRepository.GetDataEspeceAPayer();
        }

        public async Task<ApiResponse> GetEspeceAConfirmer()
        {
            return await _receptionEspecePaiementRepository.GetEspeceAConfirmerPayer();
        }
    }
}
