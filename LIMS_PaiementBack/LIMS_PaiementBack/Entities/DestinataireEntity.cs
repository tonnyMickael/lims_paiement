using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LIMS_PaiementBack.Entities
{
    [Table("Destinataire")]
    public class DestinataireEntity
    {
        [Key]
        public int idDestinataire {  get; set; }
        public string designation {  get; set; } = string.Empty;

        public List<DepartEntity> departs { get; set; } = new List<DepartEntity>();
    }
}
