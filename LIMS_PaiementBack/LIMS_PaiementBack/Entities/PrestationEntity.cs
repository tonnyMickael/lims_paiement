using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LIMS_PaiementBack.Entities
{
    [Table("Prestation")]
    public class PrestationEntity
    {
        [Key]
        public int id_prestation { get; set; }
        public int id_client { get; set; }
        public int id_etat_prestation { get; set; }        
        [Column("delaiaccorder")]
        public bool delaiaccorder { get; set; } // 0 = non accorder, 1 = accorder
        [Column("statut_paiement")]
        public bool status_paiement { get; set; } // 0 = non payé, 1 = payé
        [Column("demandeeffectuer")]
        public bool demandeEffectuer { get; set; } // 0 = non effectuée, 1 = effectuée

        [ForeignKey(nameof(id_client))]
        public ClientEntity? client { get; set; }
        // [ForeignKey(nameof(id_etat_prestation))]
        // public EtatPrestationEntity? etatPrestation { get; set; }
        
        // ⚠️ Ajout de la relation 0-1 avec EtatDecompteEntity
        public EtatDecompteEntity? EtatDecompte { get; set; }
        public ICollection<EchantillonEntity> Echantillons { get; set; } = new List<EchantillonEntity>();
    }
}
