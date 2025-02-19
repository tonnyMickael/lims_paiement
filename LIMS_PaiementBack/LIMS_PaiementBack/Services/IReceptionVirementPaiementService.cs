﻿using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Services
{
    public interface IReceptionVirementPaiementService
    {
        Task<ApiResponse> GetVirementAPayer();
        Task AddVirementPaiementRecu(RecuDto recu);
    }
}
