using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Utils;
using Microsoft.AspNetCore.Mvc;

namespace LIMS_PaiementBack.Services
{
    public interface IDemandeService
    {
        Task AddDemandeAsync(DemandeDto demande);
        Task<ApiResponse> GetDemandesAsync(int id_etat_decompte);
        Task<ApiResponse> GetDemandeListNoteAsync();
        Task<ApiResponse> VerificationOublie();
        Task<ApiResponse> GetDemandeListAfaire();
    }
}
