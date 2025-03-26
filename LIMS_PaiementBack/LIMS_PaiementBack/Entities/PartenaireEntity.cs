using System.ComponentModel.DataAnnotations;

namespace LIMS_PaiementBack.Entities
{
    public class PartenaireEntity
    {
        [Key]
        public int idPartenaire {  get; set; }
        public string nomEntreprise { get; set; } = string.Empty;
        //public int etatRelation { get; set; }
        public int etatRelation { get; set; }

        public List<SousContratEntity> sousContrat { get; set; }
        public List<ContratPartenaireEntity> contrat { get; set; }
    }
}
