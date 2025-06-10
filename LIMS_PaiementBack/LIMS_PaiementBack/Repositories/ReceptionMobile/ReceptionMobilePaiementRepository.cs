using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Utils;
using Microsoft.EntityFrameworkCore;

namespace LIMS_PaiementBack.Repositories
{
    public class ReceptionMobilePaiementRepository : IReceptionMobilePaiementRepository 
    {
        private readonly DbContextEntity _dbContext;

        public ReceptionMobilePaiementRepository(DbContextEntity dbContext)
        {
            _dbContext = dbContext;
        }

        /*
            * Ajout d'un reçu de paiement mobile dans la base de données et mise à jour de l'état du paiement et de la prestation associée.
            * @param recu : L'entité ReceptionMobileEntity représentant le reçu de paiement mobile à ajouter.
            * @return : Tâche asynchrone représentant l'opération d'ajout.
            * Cette méthode met également à jour l'état du paiement et de la prestation associée dans la base de données.
        */
        public async Task AddRecuMobilePaiement(ReceptionMobileEntity recu)
        {
            // Ajout d'une transaction pour garantir l'intégrité des opérations
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    await _dbContext.ReceptionMobile.AddAsync(recu);
                    await _dbContext.SaveChangesAsync();

                    await _dbContext.Paiement
                        .Where(x => x.idPaiement == recu.idPaiement)
                        .ExecuteUpdateAsync(setters => setters.SetProperty(e => e.EtatPaiement, true));

                    await _dbContext.Prestation
                        .Where(prestation =>
                            _dbContext.Etat_decompte
                                .Where(ed => _dbContext.Paiement
                                    .Where(p => p.idPaiement == recu.idPaiement)
                                    .Select(p => p.id_etat_decompte)
                                    .Contains(ed.id_etat_decompte)
                                )
                                .Select(ed => ed.id_prestation)
                                .Contains(prestation.id_prestation)
                        )
                        // .ExecuteUpdateAsync(setters => setters.SetProperty(p => p.status_paiement, true));
                        .ExecuteUpdateAsync(setters => setters.SetProperty(p => p.status_paiement, false));
                    
                    await _dbContext.Etat_decompte
                        .Where((ed => _dbContext.Paiement
                            .Where(p => p.idPaiement == recu.idPaiement)
                            .Select(p => p.id_etat_decompte)
                            .Contains(ed.id_etat_decompte)
                        ))
                        .ExecuteUpdateAsync(setters => setters.SetProperty(e => e.date_paiement, DateTime.Now));
                        // .ExecuteUpdateAsync(setters => setters.SetProperty(p => p.status_paiement, true));
                        // .ExecuteUpdateAsync(setters => setters.SetProperty(p => p.status_paiement, false));

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<ApiResponse> GetDataMobileAPayer()
        {            
            // Récupération des 4 dernières semaines (par date de début descendante)
            var semaines = await _dbContext.Semaine
                .OrderByDescending(s => s.DebutSemaine)
                .Take(4)
                .ToListAsync();

            List<SemaineDto> semaineDtos = new List<SemaineDto>();
            foreach (var semaine in semaines)
            {
                semaineDtos.Add(new SemaineDto
                {
                    debutSemaine = semaine.DebutSemaine,
                    finSemaine = semaine.FinSemaine,
                });
            }

            if (!semaines.Any())
            {
                return new ApiResponse
                {
                    Message = "Aucune semaine trouvée.",
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            var dateDebutGlobale = semaines.Min(s => s.DebutSemaine.Date);
            var dateFinGlobale = semaines.Max(s => s.FinSemaine.Date);

            var recue = await (
                from paiement in _dbContext.Paiement 
                join delaipaiement in _dbContext.DelaiPaiement on paiement.idPaiement equals delaipaiement.idPaiement
                join etat_decompte in _dbContext.Etat_decompte on paiement.id_etat_decompte equals etat_decompte.id_etat_decompte
                join prestation in _dbContext.Prestation on etat_decompte.id_prestation equals prestation.id_prestation
                where (paiement.id_modePaiement == 2 || delaipaiement.id_modePaiement == 2) && paiement.EtatPaiement == true
                        && paiement.DatePaiement >= dateDebutGlobale
                        && paiement.DatePaiement <= dateFinGlobale
                group new { paiement, etat_decompte } by (paiement.DatePaiement != null ? paiement.DatePaiement.Value.Date : DateTime.MinValue) into g
                select new RecuDto
                {
                    Date = g.Key,
                    NombrePaiement = g.Count(),
                    MontantTotal = g.Sum(x => (double)x.etat_decompte.total_montant * (1 - x.etat_decompte.remise / 100.0)),
                }).OrderBy(x => x.Date).ToListAsync();

            return new ApiResponse
            {
                Data = new DashboardPaiementDto
                {
                    Semaines = semaineDtos,
                    Paiements = recue
                },
                Message = "succes",
                IsSuccess = true,
                StatusCode = 200
            };        
        }

        public async Task<ApiResponse> GetMobileAConfirmerPayer()
        {
            var recue = await (
                from paiement in _dbContext.Paiement
                join etat_decompte in _dbContext.Etat_decompte on paiement.id_etat_decompte equals etat_decompte.id_etat_decompte
                join prestation in _dbContext.Prestation on etat_decompte.id_prestation equals prestation.id_prestation
                where (paiement.id_modePaiement == 2 || paiement.id_modePaiement == 4)
                        && paiement.EtatPaiement == false
                        && paiement.DatePaiement.HasValue
                        && paiement.DatePaiement.Value.Date == DateTime.Today.Date
                orderby paiement.idPaiement descending
                select new RecuDto
                {
                    referenceEtatDecompte = etat_decompte.ReferenceEtatDecompte,
                    montantApayer = FonctionGlobalUtil.MontantReel(etat_decompte.total_montant, etat_decompte.remise),
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

        public async Task<ApiResponse> GetMobileOperateur(){

            // Récupération des 4 dernières semaines (par date de début descendante)
            var semaines = await _dbContext.Semaine
                .OrderByDescending(s => s.DebutSemaine)
                .Take(4)
                .ToListAsync();
            
            List<SemaineDto> semaineDtos = new List<SemaineDto>();

            foreach (var semaine in semaines)
            {
                semaineDtos.Add(new SemaineDto
                {
                    debutSemaine = semaine.DebutSemaine,
                    finSemaine = semaine.FinSemaine,
                });
            }

            if (!semaines.Any())
            {
                return new ApiResponse
                {
                    Message = "Aucune semaine trouvée.",
                    IsSuccess = false,
                    StatusCode = 404
                };
            }
            
            var dateDebutGlobale = semaines.Min(s => s.DebutSemaine.Date);
            var dateFinGlobale = semaines.Max(s => s.FinSemaine.Date);

            var paiementParOperateur = await (
                from receptionMobile in _dbContext.ReceptionMobile
                join paiement in _dbContext.Paiement on receptionMobile.idPaiement equals paiement.idPaiement
                join etat_decompte in _dbContext.Etat_decompte on paiement.id_etat_decompte equals etat_decompte.id_etat_decompte
                join prestation in _dbContext.Prestation on etat_decompte.id_prestation equals prestation.id_prestation
                where paiement.DatePaiement >= dateDebutGlobale
                      && paiement.DatePaiement <= dateFinGlobale
                group new { paiement, receptionMobile, etat_decompte } by new
                {
                    // Date = paiement.DatePaiement.Value.Date,
                    Date = paiement.DatePaiement.HasValue ? paiement.DatePaiement.Value.Date : DateTime.MinValue,
                    Operateur = receptionMobile.operateurmobile
                } into g
                orderby g.Key.Date descending
                select new DetailMobileDto
                {
                    Date = g.Key.Date,
                    Operateur = g.Key.Operateur,
                    NombrePaiements = g.Count(),
                    MontantTotal = g.Sum(x => (double)x.etat_decompte.total_montant * (1 - x.etat_decompte.remise / 100.0)) // remplace par ton vrai champ
                }).ToListAsync();

            return new ApiResponse
            {
                Data = new DashboardDetailMobileDto
                {
                    Semaines = semaineDtos,
                    Details = paiementParOperateur
                },
                Message = "succes",
                IsSuccess = true,
                StatusCode = 200
            };
        }
    }
}