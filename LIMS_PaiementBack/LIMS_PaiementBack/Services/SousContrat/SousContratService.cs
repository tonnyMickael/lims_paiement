using LIMS_PaiementBack.Utils;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Repositories;

namespace LIMS_PaiementBack.Services
{
    public class SousContratService : ISousContratService
    {
        private readonly ISousContratRepository _sousContratRepository;

        public SousContratService(ISousContratRepository sousContratRepository)
        {
            _sousContratRepository = sousContratRepository;
        }

        /*
            * Ajoute un nouveau SousContrat avec les détails de paiement et l'ID du partenaire fournis.
            * Lève une ArgumentNullException si le paiement ou le contrat est nul.
            * 
            * @param contrat Le SousContratDto contenant les détails de paiement et du contrat.
            * @throws ArgumentNullException si le paiement ou le contrat est nul.
            * @returns Une Task représentant l'opération asynchrone.
        */
        public async Task AddSousContratAsync(SousContratDto contrat)
        {
            if (contrat.Paiement == null)
            {
                throw new ArgumentNullException(nameof(contrat.Paiement), "Paiement cannot be null.");
            }

            var paiement = new PaiementEntity
            {
                DatePaiement = contrat.Paiement.DatePaiement,
                id_modePaiement = contrat.Paiement.id_modePaiement,
                EtatPaiement = false,
                id_etat_decompte = contrat.Paiement.id_etat_decompte
            };

            // if (contrat.Contrat == null)
            // {
            //     throw new ArgumentNullException(nameof(contrat.Contrat), "Contrat cannot be null.");
            // }

            // await _sousContratRepository.AddSousContrat(paiement, contrat.Contrat.idPartenaire);
            await _sousContratRepository.AddSousContrat(paiement);
        }

        /*
            * Récupère la liste de tous les SousContrats avec leurs détails de paiement.
            * 
            * @returns Une Task contenant un ApiResponse avec la liste des SousContrats.
        */
        public async Task<ApiResponse> GetAllListeSousContrat()
        {
            return await _sousContratRepository.GetListeSousContratPaiement();
        }
        
        /*
            * Récupère la liste des SousContrats à payer avec leurs détails de paiement.
            * 
            * @returns Une Task contenant un ApiResponse avec la liste des SousContrats à payer.
        */
        public Task<ApiResponse> GetListeSousContratApayerAsync()
        {
            return _sousContratRepository.GetListeSousContratApayer();
        }

        public Task UpdateEtatPaiementSousContrat(int id_etat_decompte)
        {
            return _sousContratRepository.UpdateEtatPaiementSousContrat(id_etat_decompte);
        }
    }
}
