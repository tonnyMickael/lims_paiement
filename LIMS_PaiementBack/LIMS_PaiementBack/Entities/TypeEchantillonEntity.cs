using System.ComponentModel.DataAnnotations;

namespace LIMS_PaiementBack.Entities
{
    public class TypeEchantillonEntity
    {
        [Key]
        public int id_type_echantillon { get; set; }
        public string? designation { get; set; }

        public ICollection<EchantillonEntity> Echantillons { get; set; } = new List<EchantillonEntity>();
        //public ICollection<TypeTravauxEntity> TypeTravaux { get; set; } = new List<TypeTravauxEntity>();
        public ICollection<Type_travaux_type_echantillonEntity> TypeTravauxTypeEchantillons { get; set; } = new List<Type_travaux_type_echantillonEntity>();

    }
}
