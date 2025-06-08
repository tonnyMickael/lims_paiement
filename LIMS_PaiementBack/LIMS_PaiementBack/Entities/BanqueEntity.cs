using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LIMS_PaiementBack.Entities
{
    [Table("banque")]
    public class BanqueEntity
    {
        [Key]
        public int id_banque { get; set; }
        public string designation { get; set; } = string.Empty;

        public ICollection<OrdreDeVirementEntity> OrdreDeVirements { get; set; } = new List<OrdreDeVirementEntity>();

    }
}