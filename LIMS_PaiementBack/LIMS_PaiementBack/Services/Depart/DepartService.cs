using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Repositories.Depart;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Services.Depart
{
    public class DepartService : IDepartService
    {
        private readonly IDepartRepository _departRepository;

        public DepartService(IDepartRepository departRepository)
        {
            _departRepository = departRepository;
        }

        public async Task AddDepart(DepartDto depart)
        {
            var departEntity = new DepartEntity
            {
                DateDepart = depart.DateDepart ?? default(DateTime),
                reference = depart.reference,
                objet = depart.objet,
                idDestinataire = depart.idDestinataire
            };
            
            await _departRepository.DepartAdd(departEntity);
        }    

        public async Task<ApiResponse> GetAllDeparts(int? annee = null)
        {
            return await _departRepository.DepartGetAllDemande(annee);
        }

        public async Task<ApiResponse> GetAllDestinataire()
        {
            return await _departRepository.DestinataireGetAll();
        }
    }
}