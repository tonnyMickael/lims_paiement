using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LIMS_PaiementBack.Entities
{
    public class EtatDecompteEntity
    {
        [Key]
        public int id_etat_decompte { get; set; }
        public int id_prestation { get; set; }
        public string ReferenceEtatDecompte { get; set; } = string.Empty;
        public DateTime date_etat_decompte { get; set; }

        [ForeignKey(nameof(id_prestation))]
        public PrestationEntity? prestattion {  get; set; }

        public ICollection<Details_etat_decompte_Entity> DetailsEtatDecompte { get; set; } = new List<Details_etat_decompte_Entity>(); // Relation inverse

    }
}
