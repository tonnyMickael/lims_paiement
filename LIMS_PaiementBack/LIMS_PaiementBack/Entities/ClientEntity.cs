using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LIMS_PaiementBack.Entities
{
    [Table("Client")]
    public class ClientEntity
    {
        [Key]
        public int id_client { get; set; }
        [Column("nom")]
        public string Nom { get; set; } = string.Empty;
        [Column("adresse")]
        public string Adresse { get; set; } = string.Empty;
        [Column("cin")]
        public string CIN { get; set; } = string.Empty;
        [Column("passeport")]
        public string Passport { get; set; } = string.Empty;
        [Column("email")]
        public string Email { get; set; } = string.Empty;
        [Column("contact")]
        public string Contact { get; set; } = string.Empty;
        [Column("ref_contrat")]
        public string ref_contrat { get; set; } = string.Empty;
        [Column("isInterne")]
        public bool IsInterne { get; set; }

        //propriete de naviagation
        // Un client peut avoir plusieurs prestations (0-n)
        public ICollection<PrestationEntity> Prestations { get; set; } = new List<PrestationEntity>();
        // public ICollection<SousContratEntity> sousContrat { get; set; } = new List<SousContratEntity>();
    }
}
