using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LIMS_PaiementBack.Entities
{
    [Table("Paiement")]
    public class PaiementEntity
    {
        [Key]
        public int idPaiement { get; set; }
        public DateTime? DatePaiement { get; set; }
        public int id_modePaiement { get; set; }
        public bool EtatPaiement { get; set; }
        public string? nomDuPayant { get; set; }
        public string? prenomDuPayant { get; set; }
        public string? contactdupayant { get; set; }
        public int id_etat_decompte { get; set; }

        [ForeignKey(nameof(id_etat_decompte))]
        public EtatDecompteEntity? etatdecompte { get; set; }
        [ForeignKey(nameof(id_modePaiement))]
        public ModePaiementEntity? modepaiement { get; set; }

        public List<DelaiEntity> Delais = new List<DelaiEntity>();
        public List<SousContratEntity> sousContrat = new List<SousContratEntity>();
        public List<OrdreDeVirementEntity> ordreDeVirements = new List<OrdreDeVirementEntity>();
        public List<ReceptionEspeceEntity> receptionEspeces = new List<ReceptionEspeceEntity>();
        public List<ReceptionMobileEntity> receptionMobiles = new List<ReceptionMobileEntity>();
    }

}
