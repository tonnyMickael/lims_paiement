namespace LIMS_PaiementBack.Models
{
    public class DemandeDto
    {        
        // Attributs de `DemandeDto`
        public string? clients { get; set; }
        public string? email { get; set; }
        public string? adresse { get; set; }
        public string? contact { get; set; }
        public string? identite { get; set; }
        public string? etatDecompte { get; set; }
        public DateTime? datePaiement { get; set; }
        public int? nombreEchantillon { get; set; }

        // Attributs de `DemandePost`
        public int? reference { get; set; }
        public DateTime? dateDemande { get; set; }
        public string? objet { get; set; }
        public string? montant_literal { get; set; }
        public string? travaux { get; set; }
        public int id_destinataire { get; set; }
        
        // Attributs Etat_decompte utile pour liste
        public string? referenceEtatDecompte { get; set; }
        public DateTime? date_etat_decompte { get; set; }

        // Attributs communs
        public double? montant { get; set; }
        public int id_etat_decompte { get; set; }
    }
}
