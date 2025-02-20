using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LIMS_PaiementBack.Entities
{
    public class RefusEntity
    {
        [Key]
        public int idRefus {  get; set; }
        public int motifs { get; set; }
        public int idPaiement {  get; set; }

        [ForeignKey(nameof(idPaiement))]
        public PaiementEntity paiement { get; set; }
    }
}
