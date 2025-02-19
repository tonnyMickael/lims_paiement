using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LIMS_PaiementBack.Entities
{
    public class PaiementEntity
    {
        [Key]
        public int idPaiement { get; set; }
        public DateTime? DatePaiement { get; set; }
        public int ModePaiement { get; set; }
        public int EtatPaiement { get; set; }
        public int id_etat_decompte { get; set; }

        [ForeignKey(nameof(id_etat_decompte))]
        public EtatDecompteEntity etatdecompte { get; set; }
        public List<DelaiEntity> Delais { get; set; }
        public List<OrdreDeVirementEntity> ordreDeVirements { get; set; }
        public List<ReceptionEspeceEntity> receptionEspeces { get; set; }
        public List<ReceptionMobileEntity> receptionMobiles { get; set; }
        public List<RefusEntity> refus { get; set; }
        public List<SousContratEntity> sousContrat { get; set; }
    }

}
