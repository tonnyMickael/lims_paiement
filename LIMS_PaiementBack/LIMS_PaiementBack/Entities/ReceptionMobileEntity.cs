using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LIMS_PaiementBack.Entities
{
    public class ReceptionMobileEntity
    {
        [Key]
        public int idReceptionMobile { get; set; }
        public string Reference { get; set; } = string.Empty;
        public string? operateurmobile { get; set; }
        public int idPaiement { get; set; }

        [ForeignKey(nameof(idPaiement))]
        public PaiementEntity? Paiement { get; set; }
    }
}
