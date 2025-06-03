using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LIMS_PaiementBack.Entities
{
    [Table("demandeNoteDebit")]
    public class DemandeEntity
    {
        [Key]
        public int idDemandeNoteDebit { get; set; }
        public int reference { get; set; }
        public DateTime? DateDemande { get; set; }
        public string? objet { get; set; }
        public double montant { get; set; }
        public string? MontantLiteral { get; set; }
        public string? travaux { get; set; }
        public int id_etat_decompte { get; set; }

        [ForeignKey(nameof(id_etat_decompte))]
        public EtatDecompteEntity? etatDecompte { get; set; }
    }
}
