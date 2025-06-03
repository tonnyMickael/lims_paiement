
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LIMS_PaiementBack.Entities
{
    [Table("modepaiement")]
    public class ModePaiementEntity
    {
        [Key] 
        public int id_modepaiement { get; set; }
        public string designation { get; set; } = string.Empty;

        public List<PaiementEntity> paiements = new List<PaiementEntity>(); 
        public List<DelaiEntity> delais = new List<DelaiEntity>();
    }
}