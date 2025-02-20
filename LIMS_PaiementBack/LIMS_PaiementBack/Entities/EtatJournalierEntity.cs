using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LIMS_PaiementBack.Entities
{
    public class EtatJournalierEntity
    {
        [Key]
        public int idEtatJournalier { get; set; }
        public DateTime DateEncaissement { get; set; }
        public int Observation { get; set; }
        public int id_etat_decompte { get; set; }

        [ForeignKey(nameof(id_etat_decompte))]
        public EtatDecompteEntity etatDecompte { get; set; }
        public List<EtatHebdomadaireEntity> hebdomadaires { get; set; }
    }
}
