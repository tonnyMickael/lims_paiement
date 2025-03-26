using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Services.Depart
{
    public interface IDestinataireService
    {
        Task AddDestinataire(DestinataireDto destinataire);
        Task<ApiResponse> GetAllDestinataire(int position, int pagesize);
        Task<ApiResponse> GetModification(int id_destanataire);
        Task ModifierDestinataire(int id_destinataire,DestinataireDto destinataire);
    }
}
