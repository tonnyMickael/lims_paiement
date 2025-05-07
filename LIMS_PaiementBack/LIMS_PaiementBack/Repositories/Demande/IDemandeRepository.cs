using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Utils;
using Microsoft.AspNetCore.Mvc;

namespace LIMS_PaiementBack.Repositories
{
    public interface IDemandeRepository
    {
        Task<List<byte[]>> AddDemandeAsync(DemandeEntity demande, DemandeDto demandeDto);
        Task<ApiResponse> GetListeEtatDecomptePayer();
        Task<ApiResponse> GetDemandesAsync(int id_etat_decompte);
        Task<ApiResponse> GetAllDemandeAsync();
        Task<ApiResponse> GetVerificationAsync();
    }
}
