using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Repositories.Depart
{
    public interface IDepartRepository
    {
        Task DepartAdd(DepartEntity depart);
        Task<ApiResponse> DepartGetAllDemande();
        Task<ApiResponse> DestinataireGetAll();
    }
}