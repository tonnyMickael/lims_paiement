using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LIMS_PaiementBack.Entities
{
    public class ReceptionMobileEntity
    {
        [Key]
        public int idRecepiotnMobile { get; set; }
        public int referencce { get; set; }
        public int idPaiement { get; set; }

        [ForeignKey(nameof(idPaiement))]
        public PaiementEntity Paiement { get; set; }
    }
}
