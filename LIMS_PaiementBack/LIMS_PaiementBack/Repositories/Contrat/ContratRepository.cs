using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Utils;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;

namespace LIMS_PaiementBack.Repositories
{
    public class ContratRepository : IContratRepository
    {
        /*private readonly DbContextEntity _dbContext;

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

        public async Task AddNewPaiementContrat(PaiementEntity paiement, SousContratEntity souscontrat)
        {
            await _dbContext.Paiement.AddAsync(paiement);
            await _dbContext.SaveChangesAsync();

            var idPaiement = paiement.idPaiement;

            await _dbContext.SousContrats.AddAsync(souscontrat);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ApiResponse> AllContratPartenaire(int position, int pageSize)
        {
            try
            {
                // Nombre total d'éléments
                int totalCount = await _dbContext.ContratPartenaire.CountAsync();

                // Nombre total de pages
                int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                position = Math.Max(1, position); // S'assure que position est au moins 1

                // Requête avec pagination
                var contractuel = await _dbContext.Partenaire
                    .Join(_dbContext.ContratPartenaire,
                        partenaire => partenaire.idPartenaire,
                        contrat => contrat.idPartenaire,
                        (partenaire, contrat) => new ContratDto
                        {
                            idPartenaire = partenaire.idPartenaire,
                            nomEntreprise = partenaire.nomEntreprise,
                            etat = partenaire.etatRelation,
                            idContratPartenaire = contrat.idContratPartenaire,
                            referenceContrat = contrat.referenceContrat,
                            datePaiement = contrat.datePaiement
                        })
                    .Skip((position - 1) * pageSize) // Sauter les éléments des pages précédentes
                    .Take(pageSize) // Prendre le nombre d'éléments par page
                    .ToListAsync();

                // Construire la réponse
                var response = new ApiResponse
                {
                    Data = contractuel,
                    Message = contractuel.Any() ? "Succès" : "Aucune donnée trouvée",
                    IsSuccess = contractuel.Any(),
                    StatusCode = contractuel.Any() ? 200 : 404,
                    ViewBag = new Dictionary<string, object>
                    {
                        { "TotalCount", totalCount },
                        { "nbrPerPage", pageSize },
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
                    Message = "Erreur lors de la récupération des contrats et " + ex.Message,
                    IsSuccess = false,
                    StatusCode = 500
                };
            }
        }

        public async Task<ApiResponse> Contrat_a_modifier(int id_partenaire)
        {
            var query = await (
                from partenaire in _dbContext.Partenaire
                join contrat in _dbContext.ContratPartenaire on partenaire.idPartenaire equals contrat.idPartenaire
                where partenaire.idPartenaire == id_partenaire
                select new ContratDto
                {
                    //idPartenaire = partenaire.idPartenaire,
                    nomEntreprise = partenaire.nomEntreprise,
                    etat = partenaire.etatRelation,
                    idContratPartenaire = contrat.idContratPartenaire,
                    referenceContrat = contrat.referenceContrat,
                    datePaiement = contrat.datePaiement
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
            partenaires.nomEntreprise = contract.nomEntreprise;
            partenaires.etatRelation = contract.etat;

            // 4️⃣ Modifier les valeurs du contrat
            contratPartenaire.referenceContrat = contract.referenceContrat;
            contratPartenaire.datePaiement = contract.datePaiement;

            // 5️⃣ Sauvegarder les modifications
            await _dbContext.SaveChangesAsync();
        }*/
    }
}
