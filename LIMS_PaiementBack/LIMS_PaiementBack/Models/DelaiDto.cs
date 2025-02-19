using System.Text.Json.Serialization;

namespace LIMS_PaiementBack.Models
{
    public class DelaiDto
    {
        //attribut Etat decompte
        public int id_etat_decompte { get; set; }
        public string referenceEtatDecompte { get; set; } = string.Empty;
        public DateTime date_etat_decompte { get; set; }

        //attribut paiement
        public int idPaiement { get; set; }
        public DateTime? datePaiement { get; set; }
        public int EtatPaiement { get; set; }

        //attribut delai
        public DateTime DateFinDelai { get; set; }
        public int modePaiement { get; set; }
    }
}
