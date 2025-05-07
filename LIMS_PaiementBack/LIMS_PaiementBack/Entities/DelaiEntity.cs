using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LIMS_PaiementBack.Entities
{
    public class DelaiEntity
    {
        [Key]
        public int idDelai { get; set; }
        public DateTime DateFinDelai { get; set; }
        public int ModePaiement { get; set; }
        public int idPaiement { get; set; }

        [ForeignKey(nameof(idPaiement))]
        public PaiementEntity? Paiement { get; set; }
    }
}
