using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Utils;
using Microsoft.EntityFrameworkCore;

namespace LIMS_PaiementBack.Repositories.Depart
{
    public class DepartRepository : IDepartRepository
    {
        private readonly DbContextEntity _dbContext;

        public DepartRepository(DbContextEntity dbContext)
        {
            _dbContext = dbContext;
        }

        /*
            * Ajouter un départ
            * Vérifier l'existence d'un doublon avant d'ajouter
        */
        public async Task DepartAdd(DepartEntity depart)
        {
            // Vérifier l'existence d'un doublon
            var existingDepart = await _dbContext.Depart.FirstOrDefaultAsync(d =>
                d.objet == depart.objet &&
                d.DateDepart.Date == depart.DateDepart.Date && // Comparaison de la date seulement
                d.idDestinataire == depart.idDestinataire
            );

            if (existingDepart != null)
            {
                // Un doublon existe, donc on ne fait rien
            }

            // Récupérer la dernière référence en tant qu'entier
            var lastDepart = await _dbContext.Depart
                .OrderByDescending(d => d.reference)
                .FirstOrDefaultAsync();

            int newReference = 1; // Valeur par défaut si aucun enregistrement

            if (lastDepart != null)
            {
                newReference = lastDepart.reference + 1;
            }

            // Assigner la nouvelle référence
            depart.reference = newReference;

            // Ajouter et sauvegarder
            await _dbContext.Depart.AddAsync(depart);
            await _dbContext.SaveChangesAsync();

        }

        /*
            * Récupérer tous les départs
            * Vérifier si la liste est vide ou non
            * Retourner une réponse API avec le statut et le message appropriés
        */
        public async Task<ApiResponse> DepartGetAllDemande()
        {
            //var departs = await _dbContext.Depart.ToListAsync();

            var departListe = await (
                from depart in _dbContext.Depart 
                join destinataire in _dbContext.Destinataire on depart.idDestinataire equals destinataire.idDestinataire
                orderby depart.DateDepart descending
                select new DepartDto    
                {
                    reference = depart.reference,
                    objet = depart.objet,
                    DateDepart = depart.DateDepart,
                    designationDestinataire = destinataire.designation
                }).ToListAsync();   

            return new ApiResponse{
                Data = departListe,
                Message = departListe.Any() ? "succes" : "echec",
                IsSuccess = true,
                StatusCode = 200
            };
        }

        /*
            * Récupérer tous les destinataires
            * Vérifier si la liste est vide ou non
        */
        public async Task<ApiResponse> DestinataireGetAll()
        {
            var destinataires = await _dbContext.Destinataire.ToListAsync();

            return new ApiResponse
            {
                Data = destinataires,
                Message = destinataires.Any() ? "succes" : "echec",
                IsSuccess = true,
                StatusCode = 200
            };
        }
    }
}