using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LIMS_PaiementBack.Entities
{
    public class ReceptionEspeceEntity
    {
        [Key]
        public int idReceptionEspece { get; set; }
        public double montantRecu { get; set; }
        public int idPaiement { get; set; }

        [ForeignKey(nameof(idPaiement))]
        public PaiementEntity Paiement { get; set; }
    }
}
