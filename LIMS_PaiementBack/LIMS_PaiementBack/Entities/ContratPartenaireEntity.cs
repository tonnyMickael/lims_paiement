using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LIMS_PaiementBack.Entities
{
    public class ContratPartenaireEntity
    {
        [Key]
        public int idContratPartenaire { get; set; }
        public string referenceContrat { get; set; } = string.Empty;
        public DateTime datePaiement { get; set; }
        public int idPartenaire { get; set; }

        [ForeignKey(nameof(idPartenaire))]
        public PartenaireEntity partenaire { get; set; }
    }
}
