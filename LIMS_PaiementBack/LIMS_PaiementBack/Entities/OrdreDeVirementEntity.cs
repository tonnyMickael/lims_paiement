using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LIMS_PaiementBack.Entities
{
    public class OrdreDeVirementEntity
    {
        [Key]
        public int idOrdreDeVirement { get; set; }
        public string reference { get; set; } = string.Empty;
        public int idPaiement { get; set; }

        [ForeignKey(nameof(idPaiement))]
        public PaiementEntity paiement { get; set; }
    }
}
