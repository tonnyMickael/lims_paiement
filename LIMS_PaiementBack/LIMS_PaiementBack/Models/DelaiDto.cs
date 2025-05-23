using System.Text.Json.Serialization;

namespace LIMS_PaiementBack.Models
{
    public class DelaiDto
    {
        //attribut client 
        public string? nomClient { get; set; }

        //attribut prestation
        public double montant { get; set; }

        //attribut Etat decompte
        public int id_etat_decompte { get; set; }
        public string? referenceEtatDecompte { get; set; }
        public DateTime date_etat_decompte { get; set; }

        //attribut paiement
        public int idPaiement { get; set; }
        public DateTime? datePaiement { get; set; }
        public bool EtatPaiement { get; set; }

        //attribut delai
        public DateTime DateFinDelai { get; set; }
        public int id_modePaiement { get; set; }
        public int nombreEchantillon { get; set; }
    }
}
