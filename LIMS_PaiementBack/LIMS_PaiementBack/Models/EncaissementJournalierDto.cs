namespace LIMS_PaiementBack.Models
{
    public class EncaissementJournalierDto
    {
        public int id_etat_decompte { get; set; }
        public int idEtatJournalier { get; set; }
        public DateTime? dateEncaissement { get; set; }
        public string? EtatDecompte { get; set; }
        public string? clients { get; set; }
        public double montant { get; set; }
        public int observation { get; set; }
    }
}
