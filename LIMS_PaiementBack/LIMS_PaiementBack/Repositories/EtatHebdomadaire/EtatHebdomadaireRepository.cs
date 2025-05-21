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

        /*
            *enregistrement de la semaine
        */
        public async Task AddSemaine(SemaineEntity weeks)
        {
            await _dbContext.Semaine.AddAsync(weeks);
            await _dbContext.SaveChangesAsync();
        }

        /*
            * Cette méthode récupère les 4 dernières semaines enregistrées dans la base de données.
            * Elle utilise une requête LINQ pour trier les semaines par date de début en ordre décroissant
            * et ne prend que les 4 premières semaines.
            * 
            * Elle retourne une liste d'objets SemaineEntity contenant les détails des semaines.
        */
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

        /*
            * Cette méthode récupère tous les versements hebdomadaires pour la dernière semaine enregistrée.
            * Elle effectue plusieurs jointures entre les tables EtatHebdomadaire, Semaine, EtatJournalier, Etat_decompte, Paiement et Prestation
            * pour obtenir les informations nécessaires.
            * 
            * Elle retourne une liste d'objets VersementHebdomadaireDto contenant les détails des versements hebdomadaires.
        */
        public async Task<ApiResponse> GetAllVersementHebdomadaire()
        {
            // Récupérer la dernière semaine enregistrée, en se basant sur la date de début (ordre décroissant)
            var derniereSemaine = _dbContext.Semaine
                .OrderByDescending(s => s.DebutSemaine)
                .FirstOrDefault();// Prend la plus récente

            /* 
                * ajouter les états hebdomadaires pour la dernière semaine
                * Filtrer les Etats Journaliers correspondant à la dernière semaine
            */
            /* 
                * Si une semaine a été trouvée :
                * - On récupère tous les états journaliers compris dans cette semaine
                * - On les transforme en états hebdomadaires
                * - Et on les enregistre dans la table correspondante
            */
            if (derniereSemaine != null)
            {
                // Récupérer les états journaliers dont la date d'encaissement est comprise entre
                // le début et la fin de la dernière semaine
                // Filtrer les Etats Journaliers correspondant à la dernière semaine
                var etatsJournaliers = _dbContext.EtatJournalier
                    .Where(ej => ej.DateEncaissement >= derniereSemaine.DebutSemaine &&
                                ej.DateEncaissement <= derniereSemaine.FinSemaine)
                    .ToList();// On exécute la requête et récupère les résultats

                // Créer et ajouter les nouveaux enregistrements dans EtatHebdomadaireEntity
                 // Créer une liste d'états hebdomadaires à partir des états journaliers trouvés
                var etatsHebdomadaires = etatsJournaliers.Select(ej => new EtatHebdomadaireEntity
                {
                    idEtatJournalier = ej.idEtatJournalier, // Lien vers l'état journalier
                    idSemaine = derniereSemaine.idSemaine // Lien vers la semaine concernée
                }).ToList();

                 // Ajouter tous les états hebdomadaires créés à la base de données
                _dbContext.EtatHebdomadaire.AddRange(etatsHebdomadaires);

                // Sauvegarder les modifications dans la base de données
                _dbContext.SaveChanges();
            }
            else
            {
                // Si aucune semaine n’est enregistrée, on retourne une réponse d’échec
                return new ApiResponse
                {
                    Data = new List<VersementHebdomadaireDto>(),
                    Message = "Aucune semaine enregistrée",
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            // Récupère les 4 dernières semaines (ordre décroissant selon la date de début)
            var dernieresSemaines = _dbContext.Semaine
                .OrderByDescending(s => s.DebutSemaine)
                .Take(4)
                .ToList();
            
            // Extrait uniquement les identifiants des semaines pour faciliter la recherche plus bas
            var idSemaines = dernieresSemaines.Select(s => s.idSemaine).ToList();

            //valeur de paiement confirmer pour espece, mobile, virement
            // var etatsVoulus = new[] { 21, 22, 23 };
            var etatsVoulus = new[] { 1, 2, 3 };

            /*
                * Récupérer les états hebdomadaires pour les 4 dernières semaines
                * Effectuer des jointures entre les tables EtatHebdomadaire, Semaine, EtatJournalier, Etat_decompte, Paiement et Prestation
                * Filtrer les résultats en fonction de l'état de paiement et des semaines sélectionnées
                * Retourner une liste d'objets VersementHebdomadaireDto contenant les détails des versements hebdomadaires
            */
            var listHebdomadaire = await (
                from etatHebomadaire in _dbContext.EtatHebdomadaire 
                join semaine in _dbContext.Semaine on etatHebomadaire.idSemaine equals semaine.idSemaine
                join EtatJournalier in _dbContext.EtatJournalier on etatHebomadaire.idEtatJournalier equals EtatJournalier.idEtatJournalier
                join etatDecompte in _dbContext.Etat_decompte on EtatJournalier.id_etat_decompte equals etatDecompte.id_etat_decompte
                join paiement in _dbContext.Paiement on etatDecompte.id_etat_decompte equals paiement.id_etat_decompte
                join prestation in _dbContext.Prestation on etatDecompte.id_prestation equals prestation.id_prestation
                join client in _dbContext.Client on prestation.id_client equals client.id_client
                // Filtre : uniquement les paiements dont l'état est 21, 22 ou 23
                // et appartenant aux 4 dernières semaines
                where etatsVoulus.Contains(paiement.ModePaiement) 
                    && paiement.EtatPaiement == true
                    && idSemaines.Contains(semaine.idSemaine)
                // Projette les données dans un objet DTO personnalisé
                select new VersementHebdomadaireDto
                {
                    dateEncaissement = EtatJournalier.DateEncaissement,    // Date du versement
                    EtatDecompte = etatDecompte.ReferenceEtatDecompte,    // Référence de l'état de décompte
                    clients = client.Nom,                                // Nom du client
                    montant = FonctionGlobalUtil.MontantReel(etatDecompte.total_montant, etatDecompte.remise), // Montant après remise
                    observation = EtatJournalier.Observation    // Observations diverses
                }).ToListAsync(); // Exécute la requête de manière asynchrone

            return new ApiResponse 
            {
                Data = listHebdomadaire,
                Message = listHebdomadaire.Any() ? "succes" : "erreur", // Message dynamique selon si la liste est vide ou non
                IsSuccess = true,
                StatusCode = 200
            };
        }
    }
}
