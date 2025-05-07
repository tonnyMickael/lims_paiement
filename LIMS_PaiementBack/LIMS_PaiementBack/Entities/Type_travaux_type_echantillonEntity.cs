using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LIMS_PaiementBack.Entities
{
    public class Type_travaux_type_echantillonEntity
    {
        [Key, Column(Order = 0)]
        public int id_type_echantillon { get; set; }

        [Key, Column(Order = 1)]
        public int id_type_travaux { get; set; }

        // Navigation properties
        [ForeignKey(nameof(id_type_echantillon))]
        public TypeEchantillonEntity? TypeEchantillon { get; set; }
        [ForeignKey(nameof(id_type_travaux))]
        public TypeTravauxEntity? TypeTravaux { get; set; }
    }
}