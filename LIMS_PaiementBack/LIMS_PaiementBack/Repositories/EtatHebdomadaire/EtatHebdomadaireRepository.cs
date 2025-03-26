using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Utils;
using Microsoft.EntityFrameworkCore;

namespace LIMS_PaiementBack.Repositories.EtatHebdomadaire
{
    public class EtatHebdomadaireRepository : IEtatHebdomadaireRepository
    {
        private readonly DbContextEntity _dbContext;

        public EtatHebdomadaireRepository(DbContextEntity dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddSemaine(SemaineEntity weeks)
        {
            await _dbContext.Semaine.AddAsync(weeks);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ApiResponse> GetAllSemaine()
        {
            var dernieresSemaines = _dbContext.Semaine
                .OrderByDescending(s => s.DebutSemaine)
                .Take(4)
                .ToList();

            return new ApiResponse
            {
                Data = dernieresSemaines,
                Message = "succes",
                IsSuccess = true,
                StatusCode = 200
            };
        }

        public async Task<ApiResponse> GetAllVersementHebdomadaire()
        {
            // Récupérer la dernière semaine enregistrée
            var derniereSemaine = _dbContext.Semaine
                .OrderByDescending(s => s.DebutSemaine)
                .FirstOrDefault();

            if (derniereSemaine != null)
            {
                // Filtrer les Etats Journaliers correspondant à la dernière semaine
                var etatsJournaliers = _dbContext.EtatJournalier
                    .Where(ej => ej.DateEncaissement >= derniereSemaine.DebutSemaine &&
                                ej.DateEncaissement <= derniereSemaine.FinSemaine)
                    .ToList();

                // Créer et ajouter les nouveaux enregistrements dans EtatHebdomadaireEntity
                var etatsHebdomadaires = etatsJournaliers.Select(ej => new EtatHebdomadaireEntity
                {
                    idEtatJournalier = ej.idEtatJournalier,
                    idSemaine = derniereSemaine.idSemaine
                }).ToList();

                _dbContext.EtatHebdomadaire.AddRange(etatsHebdomadaires);
                _dbContext.SaveChanges();
            }

            var listHebdomadaire = await (
                from etatHebomadaire in _dbContext.EtatHebdomadaire 
                join semaine in _dbContext.Semaine on etatHebomadaire.idSemaine equals semaine.idSemaine
                join EtatJournalier in _dbContext.EtatJournalier on etatHebomadaire.idEtatJournalier equals EtatJournalier.idEtatJournalier
                join etatDecompte in _dbContext.Etat_decompte on EtatJournalier.id_etat_decompte equals etatDecompte.id_etat_decompte
                join paiement in _dbContext.Paiement on etatDecompte.id_etat_decompte equals paiement.id_etat_decompte
                join prestation in _dbContext.Prestation on etatDecompte.id_prestation equals prestation.id_prestation
                join client in _dbContext.Client on prestation.id_client equals client.id_client
                where paiement.EtatPaiement == 22 && semaine.idSemaine == derniereSemaine.idSemaine
                select new VersementHebdomadaireDto
                {
                    dateEncaissement = EtatJournalier.DateEncaissement,
                    EtatDecompte = etatDecompte.ReferenceEtatDecompte,
                    clients = client.Nom,
                    montant = FonctionGlobalUtil.MontantReel(prestation.total_montant, prestation.remise),
                    observation = EtatJournalier.Observation
                }).ToListAsync();

            return new ApiResponse {
                Data = listHebdomadaire,
                Message = listHebdomadaire.Any() ? "succes" : "erreur",
                IsSuccess = true,
                StatusCode = 200
            };
        }
    }
}
