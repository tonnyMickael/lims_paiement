using System.ComponentModel.DataAnnotations;

namespace LIMS_PaiementBack.Entities
{
    public class EtatPrestationEntity
    {
        [Key]
        public int id_etat_prestation { get; set; }
        public int niveau { get; set; }
        public string designation { get; set; } = string.Empty;

        public ICollection<PrestationEntity> Prestations { get; set; } = new List<PrestationEntity>();
    }
}
