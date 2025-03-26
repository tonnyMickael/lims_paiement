using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Utils;
using Microsoft.EntityFrameworkCore;

namespace LIMS_PaiementBack.Repositories
{
    public class ReceptionVirementPaiementRepository : IReceptionVirementPaiementRepository
    {
        private readonly DbContextEntity _dbContext;

        public ReceptionVirementPaiementRepository(DbContextEntity dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddRecuVirementPaiement(OrdreDeVirementEntity recu)
        {
            await _dbContext.OrdreVirement.AddAsync(recu);
            await _dbContext.SaveChangesAsync();
            await _dbContext.Paiement
                .Where(x => x.idPaiement == recu.idPaiement)
                .ExecuteUpdateAsync(setters => setters.SetProperty(e => e.EtatPaiement, 22));
        }

        public async Task<ApiResponse> GetDataVirementAPayer()
        {           
            var recue = await (
                from paiement in _dbContext.Paiement
                join etat_decompte in _dbContext.Etat_decompte on paiement.id_etat_decompte equals etat_decompte.id_etat_decompte
                where paiement.ModePaiement == 3
                orderby paiement.idPaiement descending
                select new RecuDto
                {
                    referenceEtatDecompte = etat_decompte.ReferenceEtatDecompte,
                    id_paiement = paiement.idPaiement
                }).FirstOrDefaultAsync();

            return new ApiResponse
            {
                Data = recue,
                Message = "succes",
                IsSuccess = true,
                StatusCode = 200
            };
        }
    }
}