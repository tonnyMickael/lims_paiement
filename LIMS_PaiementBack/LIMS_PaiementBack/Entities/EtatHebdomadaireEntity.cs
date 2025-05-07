using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LIMS_PaiementBack.Entities
{
    public class EtatHebdomadaireEntity
    {
        [Key]
        public int idEtatHebdomadaire { get; set; }
        public int idEtatJournalier { get; set; }
        public int idSemaine { get; set; }

        [ForeignKey(nameof(idSemaine))]
        public SemaineEntity? semaine { get; set; }
        [ForeignKey(nameof(idEtatJournalier))]
        public EtatJournalierEntity? journalier { get; set; }
    }
}
