using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LIMS_PaiementBack.Entities
{
    public class SousContratEntity
    {
        [Key]
        public int idSousContrat { get; set; }
        public int idPaiement { get; set; }
        public int idPartenaire { get; set; }

        [ForeignKey(nameof(idPaiement))]
        public PaiementEntity paiement { get; set; }
        [ForeignKey(nameof(idPartenaire))]
        public PartenaireEntity partenaire { get; set; }
    }
}
