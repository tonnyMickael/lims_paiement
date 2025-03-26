
namespace LIMS_PaiementBack.Models
{
    public class DepartDto
    {
        public int idDepart { get; set; }
        public string? reference { get; set; }
        public string? objet { get; set; }
        public DateTime? DateDepart { get; set; }

        public int idDestinataire { get; set; }
    }
}
