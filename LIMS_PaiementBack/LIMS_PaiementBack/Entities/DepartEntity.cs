using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LIMS_PaiementBack.Entities
{
    public class DepartEntity
    {
        [Key]
        public int idDepart { get; set; }
        public string reference { get; set; } = string.Empty;
        public string objet { get; set; } = string.Empty;
        public DateTime DateDepart { get; set; }
        public int idDestinataire { get; set; }

        [ForeignKey(nameof(idDestinataire))]
        public DestinataireEntity Destinataire { get; set; }
    }
}
