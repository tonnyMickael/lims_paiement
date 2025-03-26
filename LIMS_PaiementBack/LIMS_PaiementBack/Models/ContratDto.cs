namespace LIMS_PaiementBack.Models
{
    public class ContratDto
    {
        //attribut partenaire
        public string? nomEntreprise { get; set; }
        public int etat { get; set; }
        
        //attribut contratpartenaire
        public int idContratPartenaire { get; set; }
        public string? referenceContrat { get; set; }
        public DateTime datePaiement { get; set; }
        
        //attribut commun
        public int idPartenaire { get; set; }
    }
}
