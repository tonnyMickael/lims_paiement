using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS_PaiementBack.Models
{
    public class DashboardDelaiDto
    {
        public int nombreEnEspece { get; set; }
        public int nombreEnMobile { get; set; }
        public double montantEnEspece { get; set; }
        public double montantEnMobile { get; set; }
        public DateTime? dateDujour { get; set; }
    }
}