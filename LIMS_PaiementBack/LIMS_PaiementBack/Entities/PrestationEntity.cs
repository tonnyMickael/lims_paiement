using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LIMS_PaiementBack.Entities
{
    public class PrestationEntity
    {
        [Key]
        public int id_prestation { get; set; }
        public int id_client { get; set; }
        public int id_etat_prestation { get; set; }
        public decimal total_montant { get; set; }
        public double remise { get; set; }

        [ForeignKey(nameof(id_client))]
        public ClientEntity client { get; set; }

        [ForeignKey(nameof(id_etat_prestation))]
        public EtatPrestationEntity etatPrestation { get; set; }
        // ⚠️ Ajout de la relation 0-1 avec EtatDecompteEntity
        public EtatDecompteEntity? EtatDecompte { get; set; }
        public ICollection<EchantillonEntity> Echantillons { get; set; } = new List<EchantillonEntity>();
    }
}
