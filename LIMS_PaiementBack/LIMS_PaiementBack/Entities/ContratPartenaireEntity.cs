using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LIMS_PaiementBack.Entities
{
    public class ContratPartenaireEntity
    {
        [Key]
        public int idContrat { get; set; }
        public string referenceContrat { get; set; } = string.Empty;
        public DateTime dateDePaiement { get; set; }
        public int idPartenaire { get; set; }

        [ForeignKey(nameof(idPartenaire))]
        public PartenaireEntity partenaire { get; set; }
    }
}
