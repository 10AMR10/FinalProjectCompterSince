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
        public string Description { get; set; }

        public Employee? Head_Of_Unit { get; set; }

        public ICollection<Employee>? Employees { get; set; }
    }
}
