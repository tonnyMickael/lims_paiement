using Humanizer;
using System.Globalization;

namespace LIMS_PaiementBack.Utils
{
    public class FonctionGlobalUtil
    {
        // prendre l'identité du client
        public static string GetClientIdentity(string CIN, string Passport)
        {
            // Retourne le CIN si disponible, sinon le Passport, sinon "Aucune identité"
            return !string.IsNullOrEmpty(CIN) ? CIN : (Passport ?? "Aucune identité");
        }

        //calcul du montant réel avec la remise 
        public static double MontantReel(decimal total, double remise)
        {
            double TotalMontant = (double)total;
            double resultat = TotalMontant - (TotalMontant * remise / 100);
            return resultat;
        }

        //montant en lettre
        public static string ConvertirMontantEnLettres(decimal totalMontant, double remise)
        {
            decimal montantReel = (decimal)FonctionGlobalUtil.MontantReel(totalMontant, remise);
            return ((int)montantReel).ToWords(new CultureInfo("fr"));
        }

        //factorisation des designation de type d'echantillon
        public static string GetObjetEchantillon(List<string> type)
        {
            return string.Join(", ", type);
        }

    }
}
