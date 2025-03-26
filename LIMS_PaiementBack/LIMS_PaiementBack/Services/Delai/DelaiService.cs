using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Repositories;
using LIMS_PaiementBack.Utils;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace LIMS_PaiementBack.Services
{
    public class DelaiService : IDelaiService
    {
        private readonly IDelaiRepository _delaiRepository;

        public DelaiService(IDelaiRepository delaiRepository)
        {
            _delaiRepository = delaiRepository;
        }

        //ajout de nouveau délai pour un prestation
        public async Task AddDelaiAsync(DelaiDto delai)
        {
            var paiement = new PaiementEntity
            {
                DatePaiement = delai.datePaiement,
                EtatPaiement = 30,
                id_etat_decompte = delai.id_etat_decompte
            };

            var delaiPaiement = new DelaiEntity
            {
               DateFinDelai = delai.DateFinDelai,
               ModePaiement = delai.modePaiement,
               idPaiement = delai.idPaiement
            };

            await _delaiRepository.AddDelaiPaiement(delaiPaiement, paiement);
        }

        //afficher tout les délai accordée 
        public Task<ApiResponse> GetDelaiPaiement()
        {
            return _delaiRepository.GetAllDelaiAsync();
        }

        /*public Task<ApiResponse> GetDelaiAccorder(int id_etat_decompte)
        {
            return _delaiRepository.GetValidationDelai(id_etat_decompte);
        }*/

        //filtre et affichage des données pour une prestation accordé pour un délai 
        public async Task<ApiResponse> GetDelaiAccorder(int id_etat_decompte)
        {
            var data = await _delaiRepository.GetValidationDelai(id_etat_decompte);            

            //vérification si la list est null
            if (!data.IsSuccess || data.Data == null)
            {
                return new ApiResponse
                {
                    Data = null,
                    Message = "Erreur lors de la récupération des données.",
                    IsSuccess = false,
                    StatusCode = 500
                };
            }
            //verification du stat du client qui est sous contrat ou non 
            if(data.Message.ToString() == "Client sous contrat suivre procédure") 
            {
                return new ApiResponse
                {
                    Data = data,
                    Message = "Client sous contrat",
                    IsSuccess = false,
                    StatusCode = 400
                };
            }

            // Cast en liste d'objets dynamiques (car Data contient plusieurs objets)
            var prestations = ((IEnumerable<dynamic>)data.Data).ToList();

            if (prestations == null || prestations.Count == 0)
            {
                return new ApiResponse
                {
                    Data = null,
                    Message = "Aucune prestation trouvée.",
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            // Définir les périodes de 6 mois et 1 an
            DateTime dateReference = DateTime.Today;
            DateTime date6Mois = dateReference.AddMonths(-6);
            DateTime date1An = dateReference.AddYears(-1);

            // Filtrer les prestations dans les 6 derniers mois
            int totalEchantillons6Mois = prestations
                .Where(p => p.DatePaiement >= date6Mois && p.DatePaiement <= dateReference)
                .Sum(p => (int)p.NombreEchantillon);

            // Vérifier si le total atteint 600 en 6 mois
            //if (totalEchantillons6Mois >= 600)
            if (totalEchantillons6Mois >= 5)
            {
                return new ApiResponse
                {
                    Data = data.Data,
                    ViewBag = new Dictionary<string, object>
                    {
                        {"id_etat_decompte", data.ViewBag["id_etat_decompte"] },
                        {"totalEchantillon", totalEchantillons6Mois }
                    },
                    Message = "Délai accordé pour le test de période de 6 mois",
                    IsSuccess = true,
                    StatusCode = 200
                };
            }

            // Filtrer les prestations dans les 12 derniers mois
            int totalEchantillons1An = prestations
                .Where(p => p.DatePaiement >= date1An && p.DatePaiement <= dateReference)
                .Sum(p => (int)p.NombreEchantillon);

            // Vérifier si le total atteint 600 en 1 an
            //if (totalEchantillons1An >= 600)
            if (totalEchantillons1An >= 5)
            {
                return new ApiResponse
                {
                    Data = data.Data,
                    ViewBag = new Dictionary<string, object>
                    {
                        {"id_etat_decompte", data.ViewBag["id_etat_decompte"] },
                        {"totalEchantillon", totalEchantillons1An }
                    },
                    Message ="Délai accordé pour le test de période de 1 an",
                    IsSuccess = true,
                    StatusCode = 200
                };
            }

            // Si le client n'a pas assez d'échantillons dans 1 an
            return new ApiResponse
            {
                Data = null,
                ViewBag = data.ViewBag,
                Message = "Client introuvable dans le système, délai non accordé.",
                IsSuccess = false,
                StatusCode = 400
            };
        }
    }
}
