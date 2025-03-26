using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Utils;
using Microsoft.EntityFrameworkCore;

namespace LIMS_PaiementBack.Repositories.Depart
{
    public class DestinataireRepository : IDestinataireRepository
    {
        private readonly DbContextEntity _dbContext;

        public DestinataireRepository(DbContextEntity dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddNewDestinataire(DestinataireEntity depart)
        {
            await _dbContext.Destinataire.AddAsync(depart);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ApiResponse> AllDestinataire(int position, int pagesize)
        {
            try
            {
                // Nombre total d'éléments
                int totalCount = await _dbContext.Destinataire.CountAsync();

                // Nombre total de pages
                int totalPages = (int)Math.Ceiling((double)totalCount / pagesize);

                position = Math.Max(1, position); // S'assure que position est au moins 1

                // Dans ton controller ou service
                var destinataire = await _dbContext.Destinataire
                    .Select(x => new DestinataireDto
                    {
                        idDestinataire = x.idDestinataire, // Ajuste selon le nom réel
                        designation = x.designation       // Mappe "designation" à "destinataire"
                    })
                    .Skip((position - 1) * pagesize)
                    .Take(pagesize)
                    .ToListAsync();

                var response = new ApiResponse
                {
                    Data = destinataire,
                    Message = destinataire.Any() ? "Succès" : "Aucune donnée trouvée",
                    IsSuccess = destinataire.Any(),
                    StatusCode = destinataire.Any() ? 200 : 404,
                    ViewBag = new Dictionary<string, object>
                    {
                        { "TotalCount", totalCount },
                        { "nbrPerPage", pagesize },
                        { "position", position },
                        { "nbrLinks", totalPages } // Nombre total de pages
                    }
                };

                return response;
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Data = null,
                    Message = "Erreur lors de la récupération des destinataire et " + ex.Message,
                    IsSuccess = false,
                    StatusCode = 500
                };
            }
        }

        public async Task<ApiResponse> GetModificationDestinataire(int id_Destinataire)
        {
            var destinataire = await _dbContext.Destinataire
                    .Where(x => x.idDestinataire == id_Destinataire)
                    .Select(x => new DestinataireDto
                    {
                        idDestinataire = x.idDestinataire, // Ajuste selon le nom réel
                        designation = x.designation       // Mappe "designation" à "destinataire"
                    }).FirstOrDefaultAsync();

            return new ApiResponse
            {
                Data = destinataire,
                Message = "Aucune donnée trouvée",
                IsSuccess = true,
                StatusCode =  200
            };           
        }

        public async Task ModificationDestinataire(int Id_destinataire, DestinataireDto destinataire)
        {
            var destinataires = await _dbContext.Destinataire.FindAsync(Id_destinataire);

            if (destinataires == null)
            {
                throw new KeyNotFoundException("Le partenaire ou le contrat n'existe pas.");
            }

            destinataires.designation = destinataire.designation;

            await _dbContext.SaveChangesAsync();
        }
    }
}
