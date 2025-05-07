using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS_PaiementBack.Entities
{
    public class Details_etat_decompte_Entity
    {
        [Key]
        public int id_details_etat_decompte { get; set; }
        public double prix_unitaire { get; set; }
        public int id_etat_decompte { get; set; }
        public int id_type_travaux { get; set; }

        [ForeignKey(nameof(id_etat_decompte))]
        public EtatDecompteEntity? EtatDecompte { get; set; }  // Relation avec EtatDecompteEntity

        [ForeignKey(nameof(id_type_travaux))]
        public TypeTravauxEntity? TypeTravaux { get; set; }  // Relation avec TypeTravauxEntity
    }
}