using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LIMS_PaiementBack.Entities;
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

            if (lastDepart != null && int.TryParse(lastDepart.reference, out int lastReference))
            {
                newReference = lastReference + 1;
            }

            // Assigner la nouvelle référence
            depart.reference = newReference.ToString();

            // Ajouter et sauvegarder
            await _dbContext.Depart.AddAsync(depart);
            await _dbContext.SaveChangesAsync();

        }

        public async Task<ApiResponse> DepartGetAllDemande()
        {
            var departs = await _dbContext.Depart.ToListAsync();

            return new ApiResponse{
                Data = departs,
                Message = departs.Any() ? "succes" : "echec",
                IsSuccess = true,
                StatusCode = 200
            };
        }

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