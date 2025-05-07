namespace LIMS_PaiementBack.Models
{
    public class PaiementDto
    {
        //attribut affichage paiement espece
        public string? titre { get; set; } 
        public string? clients { get; set; } 
        public string? email { get; set; }
        public string? adresse { get; set; }
        public string? contact { get; set; }
        public string? identite { get; set; }
        public string? ref_contrat { get; set; }
        public double montant { get; set; }
        public string? etatDecompte { get; set; }

        //attribut paiement post
        public DateTime DatePaiement { get; set; }
        public int ModePaiement { get; set; }
        public int EtatPaiement { get; set; }
        public string? nomPayant { get; set; }
        public string? prenomPayant { get; set; }
        public int contactdupayant { get; set; }

        //attribut commun
        public int id_etat_decompte { get; set; }
    }
}
