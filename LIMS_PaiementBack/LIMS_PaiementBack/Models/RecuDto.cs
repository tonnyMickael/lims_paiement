namespace LIMS_PaiementBack.Models
{
    public class RecuDto
    {
        //reception espece
        public double montantRecu { get; set; }
        
        //reception mobile 
        public int referenceMobile { get; set; }
        public string? operateurmobile { get; set; }

        //reception virement
        public string? referenceOV { get; set; }

        //attribut commun
        public double montantApayer { get; set; }
        public string? referenceEtatDecompte { get; set; }
        public int id_paiement { get; set; }

        //attribut de recuperation dashboard 
        public DateTime Date { get; set; }
        public int NombrePaiement { get; set; }
        public double MontantTotal { get; set; }
        

        //attribut de confirmation
        public string? email { get; set; }
        public string? telephone { get; set; }
    }
}
