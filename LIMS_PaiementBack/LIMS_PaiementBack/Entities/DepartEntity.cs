using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LIMS_PaiementBack.Entities
{
    [Table("Depart")]
    public class DepartEntity
    {
        [Key]
        public int idDepart { get; set; }
        public int reference { get; set; }
        public string objet { get; set; } = string.Empty;
        public DateTime DateDepart { get; set; }
        public int idDestinataire { get; set; }

        [ForeignKey(nameof(idDestinataire))]
        public DestinataireEntity? Destinataire { get; set; }
    }
}
