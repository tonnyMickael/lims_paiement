using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Repositories;
using LIMS_PaiementBack.Utils;
using Microsoft.VisualBasic;

namespace LIMS_PaiementBack.Services
{
    public class ContratService : IContratService
    {
        /*private readonly IContratRepository _contratRepository;

        public ContratService(IContratRepository contratRepository)
        {
            _contratRepository = contratRepository;
        }

        public async Task AddContratAsync(ContratDto contrat)
        {
            var partenaire = new PartenaireEntity
            {
                nomEntreprise = contrat.nomEntreprise,
                etatRelation = contrat.etat
            };

            var contrats = new ContratPartenaireEntity
            {
                referenceContrat = contrat.referenceContrat,
                datePaiement = contrat.datePaiement
            };

            await _contratRepository.AddNewContrat(partenaire,contrats);
        }

        public async Task AddPaiementContratAsync(PaiementDto paiement, ContratDto contrat)
        {
            var paye = new PaiementEntity
            {

            };

            var souscontrat = new SousContratEntity
            {

            };
            await _contratRepository.AddNewPaiementContrat(paye, souscontrat);
        }

        public async Task<ApiResponse> GetContratPartenaire(int position, int pagesize)
        {
            return await _contratRepository.AllContratPartenaire(position, pagesize);
        }       

        public async Task<ApiResponse> Get_contrat_modifier(int id_partenaire)
        {
            return await _contratRepository.Contrat_a_modifier(id_partenaire);
        }

        public async Task ModifierContrat(int id_partenaire, int id_contrat, ContratDto contrat)
        {            
            await _contratRepository.ModificationEtatContrat(id_partenaire, id_contrat, contrat);
        }*/
    }
}
