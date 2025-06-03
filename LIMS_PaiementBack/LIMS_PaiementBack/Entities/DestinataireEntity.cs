using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LIMS_PaiementBack.Entities
{
    [Table("destinataire")]
    public class DestinataireEntity
    {
        [Key]
        [Column("id_destinataire")]
        public int idDestinataire {  get; set; }
        public string designation {  get; set; } = string.Empty;

        public List<DepartEntity> departs { get; set; } = new List<DepartEntity>();
    }
}
