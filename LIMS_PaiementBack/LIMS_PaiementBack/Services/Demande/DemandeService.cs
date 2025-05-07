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
        /*
            * Cette méthode permet d'ajouter une nouvelle demande de note de débit.
            * Elle prend en paramètre un objet DemandeDto contenant les informations de la demande.
            * Après l'ajout, elle génère un PDF et le sauvegarde dans le répertoire spécifié.
            * 
            * @param demande : L'objet DemandeDto contenant les informations de la demande.
            * @return : Tâche asynchrone qui représente l'opération d'ajout.
        */
        public async Task<List<byte[]>> AddDemandeAsync(DemandeDto demande)
        {
            var demandeNote = new DemandeEntity
            {
                DateDemande = demande.dateDemande,
                objet = demande.objet,
                montant = demande.montant ?? 0,
                MontantLiteral = demande.montant_literal,
                travaux = demande.travaux,
                id_etat_decompte = demande.id_etat_decompte
            };

            // 📌 Enregistrer dans la base de données
            var pdfbytes = await _demandeRepository.AddDemandeAsync(demandeNote,demande);
            /*
                // 📌 Générer le PDF après l'ajout
                byte[] pdfBytes = FonctionGlobalUtil.GenerateDemandePdf(demande);

                // 📌 Sauvegarder le PDF
                string directoryPath = Path.Combine("wwwroot", "pdfs");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                string brut1 = demande.etatDecompte.ToString();
                string brut2 = demande.dateDemande.ToString();

                string etatFormate = brut1
                    .Replace("\\", "-")
                    .Replace("/", "-")
                    .Replace(":", "-");
                string dateFormate = brut2
                    .Replace("\\", "-")
                    .Replace("/", "-")
                    .Replace(":", "-")
                    .Replace(" ", "-");

                string fileName = $"Demande_{etatFormate}_{dateFormate}.pdf";

                string filePath = Path.Combine(directoryPath, fileName);

                await File.WriteAllBytesAsync(filePath, pdfBytes);

                string relativeUrl = $"http://localhost:5290/pdfs/{fileName}";
            */
            return pdfbytes;
        }

        // liste des demandes de note de débit à faire
        /*
            * Cette méthode permet de récupérer la liste des demandes de note de débit à faire.
            * Elle appelle le dépôt pour obtenir la liste des demandes non encore traitées.
            * 
            * @return : Tâche asynchrone qui retourne un objet ApiResponse contenant la liste des demandes.
            *          ou un message d'erreur si aucune demande n'est trouvée.
        */
        public async Task<ApiResponse> GetDemandeListAfaire()
        {
            return await _demandeRepository.GetListeEtatDecomptePayer();
        }

        // liste des demande de note de débit déjà éffectuer
        /*
            * Cette méthode permet de récupérer la liste des demandes de note de débit déjà effectuées.
            * Elle appelle le dépôt pour obtenir la liste des demandes traitées.
            * 
            * @return : Tâche asynchrone qui retourne un objet ApiResponse contenant la liste des demandes.
            *          ou un message d'erreur si aucune demande n'est trouvée.
        */
        public async Task<ApiResponse> GetDemandeListNoteAsync()
        {
            return await _demandeRepository.GetAllDemandeAsync();
        }

        // affichage des informations de demande de note de débit suivant la procédure normal 
        /*
            * Cette méthode permet de récupérer les informations de demande de note de débit suivant la procédure normale.
            * Elle prend en paramètre l'identifiant de l'état de décompte.
            * 
            * @param id_etat_decompte : L'identifiant de l'état de décompte.
            * @return : Tâche asynchrone qui retourne un objet ApiResponse contenant les informations de la demande.
        */
        public async Task<ApiResponse> GetDemandesAsync(int id_etat_decompte)
        {
            return await _demandeRepository.GetDemandesAsync(id_etat_decompte);
        }

        // vérification des demande de note de débit non éffectuer  
        /*
            * Cette méthode permet de vérifier les demandes de note de débit non effectuées.
            * Elle appelle le dépôt pour obtenir la liste des demandes non traitées.
            * 
            * @return : Tâche asynchrone qui retourne un objet ApiResponse contenant la liste des demandes.
            *          ou un message d'erreur si aucune demande n'est trouvée.
        */
        public async Task<ApiResponse> VerificationOublie()
        {
            return await _demandeRepository.GetVerificationAsync();
        }
    }
}
