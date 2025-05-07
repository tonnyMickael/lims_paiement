
using System.ComponentModel.DataAnnotations;

namespace LIMS_PaiementBack.Entities
{
    public class SemaineEntity
    {
        [Key]
        public int idSemaine { get; set; }
        public DateTime DebutSemaine { get; set; }
        public DateTime FinSemaine { get; set; }

        public List<EtatHebdomadaireEntity> Hebdomadaire = new List<EtatHebdomadaireEntity>();
    }
}
