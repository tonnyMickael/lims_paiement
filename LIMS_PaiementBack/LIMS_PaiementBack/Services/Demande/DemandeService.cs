using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Repositories;
using LIMS_PaiementBack.Utils;
using Microsoft.AspNetCore.Mvc;

namespace LIMS_PaiementBack.Services
{
    public class DemandeService : IDemandeService
    {
        private readonly IDemandeRepository _demandeRepository;

        public DemandeService(IDemandeRepository demandeRepository)
        {
            _demandeRepository = demandeRepository;
        }

        // ajouter un nouveau demande de note de débit
        public async Task AddDemandeAsync(DemandeDto demande)
        {
            var demandeNote = new DemandeEntity
            {
                DateDemande = demande.dateDemande,
                objet = demande.objet,
                montant = demande.montant ?? 0,
                MontantLiteral = demande.montant_literal,
                id_etat_decompte = demande.id_etat_decompte
            };

            await _demandeRepository.AddDemandeAsync(demandeNote);


        }

        // liste des demandes de note de débit à faire
        public async Task<ApiResponse> GetDemandeListAfaire()
        {
            return await _demandeRepository.GetListeEtatDecomptePayer();
        }

        // liste des demande de note de débit déjà éffectuer
        public async Task<ApiResponse> GetDemandeListNoteAsync()
        {
            return await _demandeRepository.GetAllDemandeAsync();
        }

        // affichage des informations de demande de note de débit suivant la procédure normal 
        public async Task<ApiResponse> GetDemandesAsync(int id_etat_decompte)
        {
            return await _demandeRepository.GetDemandesAsync(id_etat_decompte);
        }

        // vérification des demande de note de débit non éffectuer  
        public async Task<ApiResponse> VerificationOublie()
        {
            return await _demandeRepository.GetVerificationAsync();
        }
    }
}
