using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Core.Models
{
    public class Unit
    {
        public int UnitId { get; set; }
        public string Name { get; set; }
        public string ArabicName { get; set; }
		public string ArabicDescription { get; set; }
		public string Description { get; set; }
        public ICollection<UnitCourses> unitCourses { get; set; } = new HashSet<UnitCourses>();
        
        public ICollection<UnitEmployees>? UnitEmployees { get; set; } = new HashSet<UnitEmployees>();
	}
}
