﻿using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Services
{
    public interface IContratService
    {
        Task AddContratAsync(ContratDto contrat);
        Task<ApiResponse> GetContratPartenaire();
        Task ModifierContrat(int id_partenaire, int id_contrat, ContratDto contrat);
        Task<ApiResponse> Get_contrat_modifier(int id_partenaire);
    }
}
