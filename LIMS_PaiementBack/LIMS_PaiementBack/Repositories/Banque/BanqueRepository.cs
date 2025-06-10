using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Utils;
using Microsoft.EntityFrameworkCore;

namespace LIMS_PaiementBack.Repositories
{
    public class BanqueRepository : IBanqueRepository
    {
        private readonly DbContextEntity _dbContext;

        public BanqueRepository(DbContextEntity dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddNewBanque(BanqueEntity banque)
        {
            await _dbContext.Banque.AddAsync(banque);
            await _dbContext.SaveChangesAsync();
        }      

        public async Task<ApiResponse> AllBanque(int position, int pagesize)
        {
            try
            {
                // Nombre total d'éléments
                int totalCount = await _dbContext.Banque.CountAsync();

                // Nombre total de pages
                int totalPages = (int)Math.Ceiling((double)totalCount / pagesize);

                position = Math.Max(1, position); // S'assure que position est au moins 1

                // Dans ton controller ou service
                var banque = await _dbContext.Banque
                    .Select(x => new BanqueDto
                    {
                        id_banque = x.id_banque, // Ajuste selon le nom réel
                        designation = x.designation  // Mappe "designation" à "destinataire"
                    })
                    .Skip((position - 1) * pagesize)
                    .Take(pagesize)
                    .ToListAsync();

                var response = new ApiResponse
                {
                    Data = banque,
                    Message = banque.Any() ? "Succès" : "Aucune donnée trouvée",
                    IsSuccess = banque.Any(),
                    StatusCode = banque.Any() ? 200 : 404,
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

        public async Task<ApiResponse> GetModificationBanque(int id_banque)
        {
            var banque = await _dbContext.Banque
                    .Where(x => x.id_banque == id_banque)
                    .Select(x => new BanqueDto
                    {
                        id_banque = x.id_banque, // Ajuste selon le nom réel
                        designation = x.designation       // Mappe "designation" à "destinataire"
                    }).FirstOrDefaultAsync();

            return new ApiResponse
            {
                Data = banque,
                Message = "Aucune donnée trouvée",
                IsSuccess = true,
                StatusCode =  200
            };           
        }

        public async Task ModificationBanque(int Id_banque, BanqueDto banque)
        {
            var banques = await _dbContext.Banque.FindAsync(Id_banque);
            if (banques != null)
            {
                // banques.designation = banque.designation;
                banques.designation = banque.designation ?? string.Empty;
                await _dbContext.SaveChangesAsync();
            }
        }
        
    }
}