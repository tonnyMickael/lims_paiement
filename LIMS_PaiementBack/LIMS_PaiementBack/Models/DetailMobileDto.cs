namespace LIMS_PaiementBack.Models
{
    public class DetailMobileDto
    {
       public DateTime? Date { get; set; }
       public string? Operateur { get; set; }
       public int NombrePaiements { get; set; }
       public double MontantTotal { get; set; } 
    }
}