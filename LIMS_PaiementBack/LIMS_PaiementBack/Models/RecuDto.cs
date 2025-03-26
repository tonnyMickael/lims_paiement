namespace LIMS_PaiementBack.Models
{
    public class RecuDto
    {
        //reception espece
        public double montantRecu { get; set; }
        
        //reception mobile 
        public int referenceMobile { get; set; }

        //reception virement
        public string? referenceOV { get; set; }

        //attribut commun
        public string? referenceEtatDecompte { get; set; }
        public int id_paiement { get; set; }
        public string? email { get; set; }
    }
}
