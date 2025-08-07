using Humanizer;
using LIMS_PaiementBack.Models;
using System.Globalization;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;
using System.Threading.Tasks;


namespace LIMS_PaiementBack.Utils
{
    public class FonctionGlobalUtil
    {
        // prendre l'identité du client
        public static string GetClientIdentity(string? CIN, string? Passport)
        {
            // Retourne le CIN si disponible, sinon le Passport, sinon "Aucune identité"
            return !string.IsNullOrEmpty(CIN) ? CIN : (!string.IsNullOrEmpty(Passport) ? Passport : "Aucune identité");
        }

        // Prendre l'identité du client en vérifiant chaque champ
        public static string GetClientIdentity(string? CIN, string? Passport, string? nif, string? stat)
        {
            if (!string.IsNullOrEmpty(CIN))
                return CIN;
            else if (!string.IsNullOrEmpty(Passport))
                return Passport;
            else if (!string.IsNullOrEmpty(nif) && !string.IsNullOrEmpty(stat))
                return nif + "/" + stat;
            else if (!string.IsNullOrEmpty(nif))
                return nif;
            else if (!string.IsNullOrEmpty(stat))
                return stat;
            else
                return "Aucune identité";
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

        public static string GetTravaux(List<string> travaux)
        {
            return string.Join(", ", travaux);
        }

        /*
            * Génère un PDF à partir des informations de la demande.
            * @param demande : L'objet DemandeDto contenant les informations de la demande.
            * @param referenceEtatDecompte : La référence de l'état de décompte.
            * @return : Un tableau d'octets représentant le PDF généré.
        */
        public static byte[] GenerateDemandePdf(DemandeDto demande, int referenceEtatDecompte)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            // Créez un conteneur pour le document PDF
            using var stream = new MemoryStream();

            // Utilisez QuestPDF pour générer le PDF
            // Créez le document PDF
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(50);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Text("OMNIS, DIRECTION DU LABORATOIRE");
                        row.ConstantItem(200).Text("Antananarivo, le " + demande.dateDemande?.ToString("dd/MM/yyyy")).AlignRight();
                        row.ConstantItem(200).Text("Le Directeur du Laboratiore à Monsieur Le Directeur Administratif et Financier").AlignRight();
                    });

                    page.Content().Column(col =>
                    {
                        col.Item().Element(container =>
                        {
                            container.PaddingBottom(10).Text($"N° {referenceEtatDecompte}/DLAB.").Bold();
                        });

                        col.Item().Element(container =>
                        {
                            container.PaddingBottom(15).Text("Objet : Demande d'établissement de Note de Débit")
                                .Bold()
                                .Underline();
                        });
                            
                        col.Item().Text(text =>
                        {
                            text.Span("   Veuillez établir la note de Débit correspondant à l'état de décompte de prestation ");
                            text.Span($"{demande.etatDecompte}").Bold();
                            text.Span(" ci-joint, pour ");
                            text.Span($"{demande.travaux}").Bold();
                            text.Span($" de {demande.nombreEchantillon} échantillon de type {demande.objet}.");
                        });

                        col.Item().PaddingVertical(30).Text(""); 

                        col.Item().Text($"Client : {demande.clients}");
                        col.Item().Text($"CIN/Passeport ou NIF/STAT : {demande.identite}");
                        // col.Item().Text($"NIF/STAT : {demande.nif}/{demande.stat}");
                        col.Item().Text($"Téléphone : {demande.contact}");
                        col.Item().Text($"Adresse : {demande.adresse}");
                        col.Item().Text($"Email : {demande.email}");
                        col.Item().Text($"Montant TTC : {demande.montant}({demande.montant_literal}) Ariary");

                        col.Item().PaddingVertical(40).Text("Remerciements.");
                        col.Item().PaddingVertical(50).Text("");

                        col.Item().Text($"PJ : - Etat de Décompte de Prestation N°{demande.etatDecompte} du {demande.dateDemande?.ToString("dd/MM/yyyy")}")
                            .Italic();

                        col.Item().PaddingTop(30).Text("Le Directeur du Laboratoire").AlignRight();
                    });

                    page.Footer().AlignCenter().Text("Document généré automatiquement par LIMS/DLAB");
                });
            }).GeneratePdf(stream);

            return stream.ToArray();
        }

        public static byte[] GenerateNoteDebitPdf(DemandeDto demande, List<TravauxInfo> travauxList)
        {
            var logoPath1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "tete_note_debit.jpg");
            byte[] logoBytes1 = File.ReadAllBytes(logoPath1);

            var logoPath2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "pied_note_debit.jpg");
            byte[] logoBytes2 = File.ReadAllBytes(logoPath2);

            QuestPDF.Settings.License = LicenseType.Community;

            using var stream = new MemoryStream();

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Content().Column(content =>
                    {
                        // ✅ EN-TÊTE
                        content.Item().Row(row =>
                        {
                            // Gauche
                            row.RelativeItem().Column(col =>
                            {
                                // col.Item().Text("REPOBLIKAN'I MADAGASIKARA").Bold();
                                // col.Item().Text("Fitiavana - Tanindrazana - Fandrosoana");
                                // col.Item().Text("Ministère des Mines");
                                col.Item().Width(100).Image(logoBytes1).FitWidth(); // Logo à adapter (path/logo)
                            });

                            // Droite
                            row.ConstantItem(250).Column(col =>
                            {
                                col.Item().AlignRight().Text($"Antananarivo, le {DateTime.Now:dd/MM/yyyy}");
                                col.Item().AlignRight().Text($"NOTE DE DEBIT N° _____ / OMNIS / DAF / {DateTime.Now:yy}").Bold();
                                // col.Item().AlignRight().Text(demande.clients).Bold();
                                // col.Item().AlignRight().Text($"CIN/Passeport ou NIF/STAT : {demande.identite}");
                                // col.Item().AlignRight().Text($"Tél : {demande.contact}");
                                // col.Item().AlignRight().Text($"Email : {demande.email}");
                                // col.Item().AlignRight().Text($"Adresse : {demande.adresse}");
                        
                                col.Item().AlignRight().Column(rightCol =>
                                {
                                    rightCol.Item().Text(demande.clients).Bold();
                                    rightCol.Item().Text($"CIN/Passeport ou NIF/STAT : {demande.identite}");
                                    // rightCol.Item().Text($"NIF/STAT : {demande.nif}/{demande.stat}");
                                    rightCol.Item().Text($"Tél : {demande.contact}");
                                    rightCol.Item().Text($"Email : {demande.email}");
                                    rightCol.Item().Text($"Adresse : {demande.adresse}");
                                });
                            });
                        });

                        // ✅ OBJET & REFERENCE
                        content.Item().PaddingTop(20).Text($"Objet : {demande.travaux}").Bold();
                        content.Item().Text($"Réf : Demande N° {demande.etatDecompte}/ DLAB du {demande.dateDemande?.ToString("dd/MM/yyyy")}");

                        // ✅ TABLEAU DES TRAVAUX
                        content.Item().PaddingTop(20).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(); // DESIGNATION
                                columns.ConstantColumn(60); // UNITE
                                columns.ConstantColumn(70); // NOMBRE
                                columns.ConstantColumn(80); // PU
                                columns.ConstantColumn(100); // MONTANT
                            });

                            // En-tête
                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyleHeader).Text("DESIGNATION").Bold();
                                header.Cell().Element(CellStyleHeader).Text("UNITE").Bold();
                                header.Cell().Element(CellStyleHeader).Text("NOMBRE").Bold();
                                header.Cell().Element(CellStyleHeader).Text("P.U").Bold();
                                header.Cell().Element(CellStyleHeader).Text("MONTANT en Ariary").Bold();
                            });

                            // Contenu
                            //double montantTotal = 0;
                            foreach (var travail in travauxList)
                            {
                                double montant = travail.Nombre * travail.PrixUnitaire;
                                //montantTotal += montant;

                                table.Cell().Element(CellStyle).Text(travail.Designation);
                                table.Cell().Element(CellStyle).AlignCenter().Text("UN");
                                table.Cell().Element(CellStyle).AlignCenter().Text(travail.Nombre.ToString());
                                table.Cell().Element(CellStyle).AlignRight().Text($"{travail.PrixUnitaire:N0}");
                                table.Cell().Element(CellStyle).AlignRight().Text($"{montant:N0}");
                            }

                            // Ligne total
                            table.Cell().ColumnSpan(4).Element(CellStyleHeader).AlignRight().Text("Montant Total en Ariary").Bold();
                            table.Cell().Element(CellStyleHeader).AlignRight().Text($"{demande.montant:N0}").Bold();
                        });

                        // ✅ MONTANT EN LETTRES
                        content.Item().PaddingTop(20).Text($"Arrêtée la présente Note de débit à la somme de : « {demande.montant_literal} Ariary»").Italic();

                         // ✅ COPIES
                        content.Item().PaddingTop(20).Text("Copie :").Bold();
                        content.Item().Text("- Chrono");
                        content.Item().Text("- CG");
                        content.Item().Text("- AD");
                        content.Item().Text("- Intéressé");

                        // ✅ SIGNATURE
                        content.Item().PaddingTop(90).AlignRight().Text("Le Directeur Administratif et Financier").Bold();
                    });

                    // ✅ FOOTER avec trait et texte
                    page.Footer().Column(footer =>
                    {
                        footer.Item().PaddingTop(40).ExtendHorizontal().LineHorizontal(1f);
                        // footer.Item().AlignCenter().Text("Document généré automatiquement par LIMS / DLAB").FontSize(10).Italic();
                        footer.Item().PaddingBottom(5).Image(logoBytes2, ImageScaling.FitWidth); // Logo à adapter (path/logo)
                    });                   
                });
            }).GeneratePdf(stream);

            return stream.ToArray();

            // Style des cellules du tableau
            static IContainer CellStyle(IContainer container) =>
                container.Border(1).Padding(5).AlignMiddle();

            static IContainer CellStyleHeader(IContainer container) =>
                container.Border(1).Background(Colors.Grey.Lighten2).Padding(5);
        }
    }
}
