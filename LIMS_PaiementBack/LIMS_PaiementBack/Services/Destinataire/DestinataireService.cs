using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Repositories;
using LIMS_PaiementBack.Repositories.Depart;
using LIMS_PaiementBack.Utils;
using System.Diagnostics.Contracts;

namespace LIMS_PaiementBack.Services.Depart
{
    public class DestinataireService : IDestinataireService
    {
        private readonly IDestinataireRepository _destinataireRepository;

        public DestinataireService(IDestinataireRepository destinataireRepository)
        {
            _destinataireRepository = destinataireRepository;
        }

        public async Task AddDestinataire(DestinataireDto destinataire)
        {
            var destinataires = new DestinataireEntity
            {
                designation = destinataire.designation
            };

            await _destinataireRepository.AddNewDestinataire(destinataires);

        }

        public async Task<ApiResponse> GetAllDestinataire(int position, int pagesize)
        {
            return await _destinataireRepository.AllDestinataire(position, pagesize);
        }

        public async Task<ApiResponse> GetModification(int id_destanataire)
        {
            return await _destinataireRepository.GetModificationDestinataire(id_destanataire);
        }

        public async Task ModifierDestinataire(int Id_destinataire, DestinataireDto destinataire)
        {
            await _destinataireRepository.ModificationDestinataire(Id_destinataire, destinataire);
        }
    }
}
