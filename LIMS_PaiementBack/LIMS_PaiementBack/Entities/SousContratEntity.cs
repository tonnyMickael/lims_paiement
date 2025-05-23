using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LIMS_PaiementBack.Entities
{
    [Table("SousContrats")]
    public class SousContratEntity
    {
        [Key]
        public int idSousContrat { get; set; }
        public int idPaiement { get; set; }
        // public int idPartenaire { get; set; }
        // public int id_client { get; set; }

        [ForeignKey(nameof(idPaiement))]
        public PaiementEntity? paiement { get; set; }
        // [ForeignKey(nameof(idPartenaire))]
        // public PartenaireEntity? partenaire { get; set; }
        // [ForeignKey(nameof(id_client))]
        // public ClientEntity? client { get; set; }

    }
}
