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
        public string? nomDuPayant { get; set; }
        public string? prenomDuPayant { get; set; }
        public int contactdupayant { get; set; }
        public int id_etat_decompte { get; set; }

        [ForeignKey(nameof(id_etat_decompte))]
        public EtatDecompteEntity? etatdecompte { get; set; }
        
        public List<DelaiEntity> Delais = new List<DelaiEntity>();
        public List<OrdreDeVirementEntity> ordreDeVirements = new List<OrdreDeVirementEntity>();
        public List<ReceptionEspeceEntity> receptionEspeces = new List<ReceptionEspeceEntity>();
        public List<ReceptionMobileEntity> receptionMobiles = new List<ReceptionMobileEntity>();
        public List<SousContratEntity> sousContrat = new List<SousContratEntity>();
    }

}
