using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LIMS_PaiementBack.Entities
{
    [Table("Depart")]
    public class DepartEntity
    {
        [Key]
        [Column("id_depart")]
        public int idDepart { get; set; }
        [Column("reference")]
        public int reference { get; set; }
        [Column("objet")]
        public string objet { get; set; } = string.Empty;
        [Column("date_depart")]
        public DateTime DateDepart { get; set; }
        [Column("id_destinataire")]
        public int idDestinataire { get; set; }

        [ForeignKey(nameof(idDestinataire))]
        public DestinataireEntity? Destinataire { get; set; }
    }
}
