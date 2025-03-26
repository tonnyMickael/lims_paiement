using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Services.Depart
{
    public interface IDepartService
    {
        Task<ApiResponse> GetAllDeparts();
        Task<ApiResponse> GetAllDestinataire();
        Task AddDepart(DepartDto depart);
    }
}