namespace LIMS_PaiementBack.Models
{
    public class SousContratDto
    {
       /* //attribut de paiement
        public DateTime datePaiement { get; set; }
        public int modePaiement { get; set; }
        public int etatPaiement { get; set; }
        public int id_etat_decompte { get; set; }

        //souscontrat
        public int idPaiement { get; set; }
        public int idPartenaire { get; set; }*/


        public PaiementDto? Paiement { get; set; }
        public ContratDto? Contrat { get; set; }
    }
}
