namespace LIMS_PaiementBack.Models
{
    public class DashboardPaiementDto
    {
        public List<RecuDto>? Paiements { get; set; }
        public List<SemaineDto>? Semaines { get; set; }        
    }
}