using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Utils;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;

namespace LIMS_PaiementBack.Repositories
{
    public class ContratRepository : IContratRepository
    {
        private readonly DbContextEntity _dbContext;

        public ContratRepository(DbContextEntity dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddNewContrat(PartenaireEntity partenaire, ContratPartenaireEntity contrat)
        {
            // Ajoute d'abord le partenaire et enregistre pour générer l'ID
            await _dbContext.Partenaire.AddAsync(partenaire);
            await _dbContext.SaveChangesAsync(); // L'ID est généré ici

            // Maintenant que l'ID existe, on l'affecte au contrat
            contrat.idPartenaire = partenaire.idPartenaire;

            // Ajout du contrat avec l'ID du partenaire
            await _dbContext.ContratPartenaire.AddAsync(contrat);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ApiResponse> AllContratPartenaire()
        {
            var contractuel = await (
                from partenaire in _dbContext.Partenaire
                join contrat in _dbContext.ContratPartenaire on partenaire.idPartenaire equals contrat.idPartenaire
                select new ContratDto
                {
                    idPartenaire = partenaire.idPartenaire,
                    nomEntreprice = partenaire.nomEntreprise,
                    etat = partenaire.etat,
                    idContratPartenaire = contrat.idContrat,
                    referenceContrat = contrat.referenceContrat,
                    datePaiement = contrat.dateDePaiement
                }).ToListAsync();

            return new ApiResponse
            {
                Data = contractuel,
                Message = contractuel.Any() ? "succes" : "Aucune donnée trouvée",
                IsSuccess = true,
                StatusCode = 200
            };          
        }

        public async Task<ApiResponse> Contrat_a_modifier(int id_partenaire)
        {
            var query = await (
                from partenaire in _dbContext.Partenaire
                join contrat in _dbContext.ContratPartenaire on partenaire.idPartenaire equals contrat.idPartenaire
                where partenaire.idPartenaire == id_partenaire
                select new ContratDto
                {
                    idPartenaire = partenaire.idPartenaire,
                    nomEntreprice = partenaire.nomEntreprise,
                    etat = partenaire.etat,
                    idContratPartenaire = contrat.idContrat,
                    referenceContrat = contrat.referenceContrat,
                    datePaiement = contrat.dateDePaiement
                }).FirstOrDefaultAsync();

            return new ApiResponse
            {
                Data = query,
                Message = "succes",
                IsSuccess = true,
                StatusCode = 200
            };
        }

        public async Task ModificationEtatContrat(int id_partenaire, int id_contrat, ContratDto contract)
        {
            // 1️⃣ Récupérer les entités existantes
            var partenaires = await _dbContext.Partenaire.FindAsync(id_partenaire);
            var contratPartenaire = await _dbContext.ContratPartenaire.FindAsync(id_contrat);

            // 2️⃣ Vérifier si les entités existent
            if (partenaires == null || contratPartenaire == null)
            {
                throw new KeyNotFoundException("Le partenaire ou le contrat n'existe pas.");
            }

            // 3️⃣ Modifier les valeurs du partenaire
            partenaires.nomEntreprise = contract.nomEntreprice;
            partenaires.etat = contract.etat;

            // 4️⃣ Modifier les valeurs du contrat
            contratPartenaire.referenceContrat = contract.referenceContrat;
            contratPartenaire.dateDePaiement = contract.datePaiement;

            // 5️⃣ Sauvegarder les modifications
            await _dbContext.SaveChangesAsync();
        }
    }
}
