using LIMS_PaiementBack.Utils;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Entities;
using Microsoft.EntityFrameworkCore;

namespace LIMS_PaiementBack.Repositories
{
    public class SousContratRepository : ISousContratRepository
    {
        private readonly DbContextEntity _dbContext;
        
        public SousContratRepository(DbContextEntity dbContext)
        {
            _dbContext = dbContext;
        }

        /*
            * Ajoute un nouveau SousContrat avec les détails de paiement et l'ID du partenaire fournis.
            * 
            * @param paiement Le PaiementEntity contenant les détails de paiement.
            * @param id_partenaire L'ID du partenaire associé au SousContrat.
            * @returns Une Task représentant l'opération asynchrone.
            * @throws DbUpdateException si une erreur se produit lors de l'enregistrement dans la base de données.        
        */
        // public async Task AddSousContrat(PaiementEntity paiement, int id_partenaire)
        public async Task AddSousContrat(PaiementEntity paiement)
        {
            // Ajout d'une transaction pour garantir l'intégrité des opérations
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    await _dbContext.Paiement.AddAsync(paiement);
                    await _dbContext.SaveChangesAsync();

                    var contrat = new SousContratEntity
                    {
                        idPaiement = paiement.idPaiement,
                        // idPartenaire = id_partenaire
                    };

                    await _dbContext.SousContrats.AddAsync(contrat);
                    await _dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            /*
                // await _dbContext.Prestation
                //     .Where(prestation =>
                //         _dbContext.Etat_decompte
                //             .Where(ed => _dbContext.Paiement
                //                 .Where(p => p.idPaiement == paiement.idPaiement)
                //                 .Select(p => p.id_etat_decompte)
                //                 .Contains(ed.id_etat_decompte)
                //             )
                //             .Select(ed => ed.id_prestation)
                //             .Contains(prestation.id_prestation)
                //     )
                //     .ExecuteUpdateAsync(setters => setters.SetProperty(p => p.status_paiement, 5));
            */
        }

        /*
            * Récupère la liste de tous les SousContrats avec leurs détails de paiement.
            * 
            * @returns Une Task contenant un ApiResponse avec la liste des SousContrats.       
        */
        public async Task<ApiResponse> GetListeSousContratApayer()
        {
            var sousContratList = await (
                from sousContrat in _dbContext.SousContrats
                join paiement in _dbContext.Paiement on sousContrat.idPaiement equals paiement.idPaiement
                join etatDecompte in _dbContext.Etat_decompte on paiement.id_etat_decompte equals etatDecompte.id_etat_decompte
                join prestation in _dbContext.Prestation on etatDecompte.id_prestation equals prestation.id_prestation
                join client in _dbContext.Client on prestation.id_client equals client.id_client
                // join partenaire in _dbContext.Partenaire on sousContrat.idPartenaire equals partenaire.idPartenaire
                // join contrat in _dbContext.ContratPartenaire on partenaire.idPartenaire equals contrat.idPartenaire
                where paiement.id_modePaiement == 4 && paiement.EtatPaiement == false // Etat de paiement pour le sous contrat (non payé)
                select new SousContratDto
                {
                    Paiement = new PaiementDto
                    {
                        id_etat_decompte = etatDecompte.id_etat_decompte,
                        clients = client.Nom,
                        ref_contrat = client.ref_contrat,
                        montant = FonctionGlobalUtil.MontantReel(etatDecompte.total_montant, etatDecompte.remise),
                        etatDecompte = etatDecompte.ReferenceEtatDecompte,
                        DatePaiement = paiement.DatePaiement ?? default(DateTime)

                    }
                    // Contrat = new ContratDto
                    // {
                    //     nomEntreprise = client.Nom,
                    //     referenceContrat = contrat.referenceContrat,
                    //     datePaiement = contrat.datePaiement
                    // }
                }
            ).ToListAsync();

            return new ApiResponse
            {
                Data = sousContratList,
                Message = sousContratList.Any() ? "succès" : "donnée irrécupérable",
                IsSuccess = true,
                StatusCode = 200
            };
        }

        /*
            * Récupère la liste de tous les SousContrats avec leurs détails de paiement.
            * 
            * @returns Une Task contenant un ApiResponse avec la liste des SousContrats.       
        */
        public async Task<ApiResponse> GetListeSousContratPaiement()
        {
            var listeSousContrat = await (
                from SousContrat in _dbContext.SousContrats
                join paiement in _dbContext.Paiement on SousContrat.idPaiement equals paiement.idPaiement
                join etat_decompte in _dbContext.Etat_decompte on paiement.id_etat_decompte equals etat_decompte.id_etat_decompte
                join prestation in _dbContext.Prestation on etat_decompte.id_prestation equals prestation.id_prestation
                join client in _dbContext.Client on prestation.id_client equals client.id_client
                // join partenaire in _dbContext.Partenaire on SousContrat.idPartenaire equals partenaire.idPartenaire
                // join contrat in _dbContext.ContratPartenaire on partenaire.idPartenaire equals contrat.idPartenaire
                select new SousContratDto
                {
                    Paiement = new PaiementDto
                    {                        
                        clients = client.Nom,                        
                        contact = client.Contact,                  
                        montant = FonctionGlobalUtil.MontantReel(etat_decompte.total_montant, etat_decompte.remise),
                        etatDecompte = etat_decompte.ReferenceEtatDecompte,
                        DatePaiement = paiement.DatePaiement ?? default(DateTime),  
                        prenomPayant = paiement.prenomDuPayant,
                        contactdupayant = paiement.contactdupayant,
                    }
                    // Contrat = new ContratDto
                    // {
                    //     nomEntreprise = partenaire.nomEntreprise,                        
                    //     referenceContrat = contrat.referenceContrat,
                    //     datePaiement = contrat.datePaiement,
                    // }
                }).ToListAsync();


            return new ApiResponse
            {
                Data = listeSousContrat,
                Message = listeSousContrat.Any() ? "succces" : "donnée irrécupérable",
                IsSuccess = true,
                StatusCode = 200
            };
        }

        public async Task UpdateEtatPaiementSousContrat(int id_etat_decompte)
        {
            await _dbContext.Prestation
                .Where(prestation => _dbContext.Etat_decompte
                    .Where(ed => ed.id_etat_decompte == id_etat_decompte)
                    .Select(ed => ed.id_prestation)
                    .Contains(prestation.id_prestation)
                )
                .ExecuteUpdateAsync(setters => setters.SetProperty(p => p.status_paiement, true));
                // .ExecuteUpdateAsync(setters => setters.SetProperty(p => p.status_paiement, false));

            var paiement = await _dbContext.Paiement.FirstOrDefaultAsync(p => p.id_etat_decompte == id_etat_decompte);
            if (paiement != null)
            {
                paiement.EtatPaiement = true; // Mettre à jour l'état de paiement pour le sous contrat (payé)
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("État de paiement introuvable.");
            }
        }
    }
}
