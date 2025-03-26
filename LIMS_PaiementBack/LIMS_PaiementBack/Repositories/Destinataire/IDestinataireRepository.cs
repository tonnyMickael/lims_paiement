using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Repositories.Depart
{
    public interface IDestinataireRepository
    {
        Task AddNewDestinataire(DestinataireEntity depart);
        Task<ApiResponse> AllDestinataire(int position, int pagesize);
        Task<ApiResponse> GetModificationDestinataire(int id_Destinataire);
        Task ModificationDestinataire(int Id_destinataire, DestinataireDto destinataire);
    }
}
