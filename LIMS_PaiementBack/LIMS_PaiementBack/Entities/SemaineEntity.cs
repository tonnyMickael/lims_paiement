using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LIMS_PaiementBack.Entities
{
    [Table("semaine")]
    public class SemaineEntity
    {
        [Key]
        public int idSemaine { get; set; }
        public DateTime DebutSemaine { get; set; }
        public DateTime FinSemaine { get; set; }

        public List<EtatHebdomadaireEntity> Hebdomadaire = new List<EtatHebdomadaireEntity>();
    }
}
