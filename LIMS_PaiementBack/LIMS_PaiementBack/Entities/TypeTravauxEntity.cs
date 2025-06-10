using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LIMS_PaiementBack.Entities
{
    [Table("type_travaux")]
    public class TypeTravauxEntity
    {
        [Key]
        public int id_type_travaux { get; set; }        
        public string designation { get; set; } = string.Empty;

        public ICollection<Details_etat_decompte_Entity> DetailsEtatDecompte { get; set; } = new List<Details_etat_decompte_Entity>(); // Relation inverse
        public ICollection<Type_travaux_type_echantillonEntity> TypeTravauxTypeEchantillons { get; set; } = new List<Type_travaux_type_echantillonEntity>();
    }
}