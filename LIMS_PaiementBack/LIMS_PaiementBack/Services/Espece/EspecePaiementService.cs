using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Repositories;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Services
{
    public class EspecePaiementService : IEspecePaiementService
    {
        private readonly IEspecePaiementRepository _especePaiementRepository;
        // private readonly Email _email;

        public EspecePaiementService(IEspecePaiementRepository especePaiementRepository)
        {
            _especePaiementRepository = especePaiementRepository;
            // _email = email;
        }

        public async Task AddEspecePaiement(PaiementDto paiement)
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

            await _especePaiementRepository.AddPaiementEspece(paiements);

            // Envoyer un email si l'adresse email est renseignée
            // if (!string.IsNullOrEmpty(paiement.email))
            // {
            //     await _email.SendEmailAsync(paiement.email, "Confirmation de paiement",
            //         $"Votre paiement de {paiement.montant} a été reçu avec succès.");
            // }
        }

        public async Task<ApiResponse> GetInfoEspecePaiement(int id_etat_decompte)
        {
            return await _especePaiementRepository.GetDataPaiementEspece(id_etat_decompte);
        }
    }
}
