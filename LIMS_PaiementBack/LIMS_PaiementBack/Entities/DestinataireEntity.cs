using System.ComponentModel.DataAnnotations;

namespace LIMS_PaiementBack.Entities
{
    [Table("destinataire")]
    public class DestinataireEntity
    {
        [Key]
        public int idDestinataire {  get; set; }
        public string designation {  get; set; } = string.Empty;

        public List<DepartEntity> departs { get; set; } = new List<DepartEntity>();
    }
}
