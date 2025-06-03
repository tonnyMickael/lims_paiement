using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LIMS_PaiementBack.Entities
{
    [Table("delaipaiement")]
    public class DelaiEntity
    {
        [Key]
        public int idDelai { get; set; }
        public DateTime DateFinDelai { get; set; }
        public int id_modePaiement{ get; set; }
        public int idPaiement { get; set; }

        [ForeignKey(nameof(idPaiement))]
        public PaiementEntity? Paiement { get; set; }
        [ForeignKey(nameof(id_modePaiement))]
        public ModePaiementEntity? ModePaiement { get; set; }
    }
}
