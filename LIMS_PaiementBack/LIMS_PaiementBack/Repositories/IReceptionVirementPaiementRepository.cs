﻿using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Repositories
{
    public interface IReceptionVirementPaiementRepository
    {
        Task AddRecuVirementPaiement(OrdreDeVirementEntity recu);
        Task<ApiResponse> GetDataVirementAPayer();
    }
}
