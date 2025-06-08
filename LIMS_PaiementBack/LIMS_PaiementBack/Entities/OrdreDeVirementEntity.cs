using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LIMS_PaiementBack.Entities
{
    [Table("ordredevirement")]

    public class OrdreDeVirementEntity
    {
        [Key]
        public int idOrdreDeVirement { get; set; }
        public string reference { get; set; } = string.Empty;
        public int id_banque { get; set; }
        public int idPaiement { get; set; }

        [ForeignKey(nameof(idPaiement))]
        public PaiementEntity? paiement { get; set; }
        [ForeignKey(nameof(id_banque))]
        public BanqueEntity? banque { get; set; }
    }
}
