using System.ComponentModel.DataAnnotations;

namespace LIMS_PaiementBack.Entities
{
    public class ClientEntity
    {
        [Key]
        public int id_client { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Adresse { get; set; } = string.Empty;
        public string CIN { get; set; } = string.Empty;
        public string Passport { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        public string ref_contrat { get; set; } = string.Empty;
        public bool IsInterne { get; set; }

        //propriete de naviagation
        // Un client peut avoir plusieurs prestations (0-n)
        public ICollection<PrestationEntity> Prestations { get; set; } = new List<PrestationEntity>();
        // public ICollection<SousContratEntity> sousContrat { get; set; } = new List<SousContratEntity>();
    }
}
