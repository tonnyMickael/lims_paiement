using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LIMS_PaiementBack.Repositories
{
    public class DemandeRepository : IDemandeRepository
    {
        private readonly DbContextEntity _dbContext;

        public DemandeRepository(DbContextEntity dbContext)
        {
            _dbContext = dbContext;
        }

        // ajout de nouveau demande de note de débit
        public async Task AddDemandeAsync(DemandeEntity demande)
        {
            await _dbContext.DemandeNoteDebit.AddAsync(demande);
            await _dbContext.SaveChangesAsync();            
        }

        // liste des demande de note de débit éffectuer
        public async Task<ApiResponse> GetAllDemandeAsync()
        {
            var demandeList = await (
                from etat_decompte in _dbContext.Etat_decompte
                join demande in _dbContext.DemandeNoteDebit on etat_decompte.id_etat_decompte equals demande.id_etat_decompte
                    select new DemandeDto
                    {
                        reference = demande.reference,
                        dateDemande = demande.DateDemande,
                        objet = demande.objet,
                        montant = demande.montant,
                        montant_literal = demande.MontantLiteral,
                        referenceEtatDecompte = etat_decompte.ReferenceEtatDecompte,
                        date_etat_decompte = etat_decompte.date_etat_decompte,
                        id_etat_decompte = etat_decompte.id_etat_decompte
                    }).ToListAsync();

            var rendu = new ApiResponse
            {
                Data = demandeList,
                Message = demandeList.Any() ? "succes" : "Aucune donnée trouvé",
                IsSuccess = true,
                StatusCode = 200
            };
            return rendu;
        }

        // affichage des informations de demande de note de débit suivant la procédure normal
        public async Task<ApiResponse> GetDemandesAsync(int id_etat_decompte)
        {     
            var demande = await (from client in _dbContext.Client
                join prestation in _dbContext.Prestation on client.id_client equals prestation.id_client
                join etatDecompte in _dbContext.Etat_decompte on prestation.id_prestation equals etatDecompte.id_prestation
                join echantillon in _dbContext.Echantillon on prestation.id_prestation equals echantillon.id_prestation into echantillonsGroup
                where etatDecompte.id_etat_decompte == id_etat_decompte
                select new DemandeDto
                {
                    clients = client.Nom,
                    email = client.Email,
                    adresse = client.Adresse,
                    contact = client.Contact,
                    identite = FonctionGlobalUtil.GetClientIdentity(client.CIN, client.Passport), // récuperation de d'identité du client
                    etatDecompte = etatDecompte.ReferenceEtatDecompte,
                    datePaiement = DateTime.Now,
                    id_etat_decompte = etatDecompte.id_etat_decompte,
                    montant = FonctionGlobalUtil.MontantReel(prestation.total_montant,prestation.remise),// récuperation du montant réel à payer
                    nombreEchantillon = echantillonsGroup.Count()
                }).ToListAsync();

            var result = new ApiResponse
            {
                Data = demande,
                Message = demande.Any() ? "Succès" : "Aucune donnée trouvée",
                IsSuccess = demande.Any(),
                StatusCode = 200
            };

            return result;
        }

        // liste des demandes de note de débit à faire 
        public async Task<ApiResponse> GetListeEtatDecomptePayer()
        {
            var liste = await (
                from etat_prestation in _dbContext.Etat_prestation
                join prestation in _dbContext.Prestation on etat_prestation.id_etat_prestation equals prestation.id_etat_prestation
                join etat_decompte in _dbContext.Etat_decompte on prestation.id_prestation equals etat_decompte.id_prestation
                where etat_prestation.id_etat_prestation == 2
                select new
                {
                    id_etat_decompte = etat_decompte.id_etat_decompte,
                    referenceEtatDecompte = etat_decompte.ReferenceEtatDecompte,
                    date_etat_decompte = etat_decompte.date_etat_decompte,
                    montant = FonctionGlobalUtil.MontantReel(prestation.total_montant, prestation.remise)// récuperation du montant réel à payer
                }).ToListAsync();

            return new ApiResponse
            {
                Data = liste,
                Message = liste.Any() ? "succès" : "aucune donnée trouvé",
                IsSuccess = true,
                StatusCode = 200
            };
        }

        // vérification des demande de note de débit non éffectuer 
        public async Task<ApiResponse> GetVerificationAsync()
        {
            // Date fixe pour les tests, sinon utiliser DateTime.Today
            //DateTime today = DateTime.Today;
            DateTime today = new DateTime(2025, 01, 21);

            // 1️⃣ Récupérer les ID des EtatDecompte du jour
            var etatDecompteJour = await (
                from etat_prestation in _dbContext.Etat_prestation
                join prestation in _dbContext.Prestation on etat_prestation.id_etat_prestation equals prestation.id_etat_prestation
                join etat_decompte in _dbContext.Etat_decompte on prestation.id_prestation equals etat_decompte.id_prestation
                where etat_prestation.niveau == 1 && etat_decompte.date_etat_decompte == today
                select new
                {
                    id_etat_decompte = etat_decompte.id_etat_decompte,
                    referenceEtatDecompte = etat_decompte.ReferenceEtatDecompte,
                    date_etat_decompte = etat_decompte.date_etat_decompte,
                    montant = FonctionGlobalUtil.MontantReel(prestation.total_montant, prestation.remise)
                }).ToListAsync();

            if (!etatDecompteJour.Any())
            {
                return new ApiResponse
                {
                    Data = null,
                    Message = "Aucun état de décompte trouvé pour aujourd'hui.",
                    IsSuccess = false,
                    StatusCode = 404
                };
            }

            // 2️⃣ Récupérer les ID déjà enregistrés dans DemandeNoteDebit
            var idDejaEnregistre = await _dbContext.DemandeNoteDebit
                .Where(d => d.DateDemande.HasValue && d.DateDemande.Value.Date == today)
                .Select(d => d.id_etat_decompte)
                .ToListAsync();

            // 3️⃣ Filtrer ceux qui ne sont pas encore enregistrés
            var Manquant = etatDecompteJour
                .Where(e => !idDejaEnregistre.Contains(e.id_etat_decompte))
                .Select(e => new DemandeDto
                {
                    id_etat_decompte = e.id_etat_decompte,
                    referenceEtatDecompte = e.referenceEtatDecompte,
                    dateDemande = e.date_etat_decompte,
                    montant = e.montant
                }).ToList();

            return new ApiResponse
            {
                Data = Manquant,
                Message = Manquant.Any() ? "⚠️ Vous avez oublié de faire ces demandes !" : "✅ Rien oublié.",
                IsSuccess = true,
                StatusCode = 200
            };
        }
    }
}
