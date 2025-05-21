using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LIMS_PaiementBack.Entities
{
    [Table("echantillon")]
    public class EchantillonEntity
    {
        [Key]
        public int id_echantillon { get; set; }
        public int id_type_echantillon { get; set; }
        public int id_prestation { get; set; }

        [ForeignKey(nameof(id_prestation))]
        public PrestationEntity? prestation { get; set; }

        [ForeignKey(nameof(id_type_echantillon))]
        public TypeEchantillonEntity? typeEchantillon { get; set; }
    }
}
