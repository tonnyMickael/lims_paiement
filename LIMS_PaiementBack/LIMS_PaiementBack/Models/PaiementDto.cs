namespace LIMS_PaiementBack.Models
{
    public class PaiementDto
    {
        //attribut affichage paiement espece
        public string titre { get; set; } = string.Empty;
        public string clients { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string adresse { get; set; } = string.Empty;
        public string contact { get; set; } = string.Empty;
        public string identite { get; set; } = string.Empty;
        public double montant { get; set; }
        public string etatDecompte { get; set; } = string.Empty;

        //attribut paiement espece post
        public DateTime DatePaiement { get; set; }
        public int ModePaiement { get; set; }
        public int EtatPaiement { get; set; }
       
        //attribut commun
        public int id_etat_decompte { get; set; }
    }
}
