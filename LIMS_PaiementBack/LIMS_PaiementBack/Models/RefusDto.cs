namespace LIMS_PaiementBack.Models
{
    public class RefusDto
    {
        // attribut du refus
        public int motifs { get; set; }
        //public string motifs { get; set; }
        
        // attribut paiement
        public DateTime DatePaiement { get; set; }
        public int ModePaiement { get; set; }
        public int EtatPaiement { get; set; }

        //attribut etat decompte 
        public string? referenceEtatDecompte { get; set; } 

        //attribut commun
        public int id_paiement { get; set; }
        public int id_etat_decompte { get; set; }

    }
}
