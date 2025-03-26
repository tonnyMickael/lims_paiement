namespace LIMS_PaiementBack.Models
{
    public class DemandeDto
    {
        /*public string clients { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string adresse { get; set; } = string.Empty;
        public string contact { get; set; } = string.Empty;
        public string identite { get; set; } = string.Empty;
        public string etatDecompte { get; set; } = string.Empty;
        public DateTime datePaiement { get; set; }
        public int idEtatDecompte { get; set; }
        public double montant { get; set; }
        public int nombreEchantillon { get; set; }*/
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
        
        // Attributs Etat_decompte utile pour liste
        public string? referenceEtatDecompte { get; set; }
        public DateTime? date_etat_decompte { get; set; }


        // Attributs communs
        public double? montant { get; set; }
        public int id_etat_decompte { get; set; }

    }
}
