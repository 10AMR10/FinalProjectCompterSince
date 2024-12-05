using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Core.Dtos.UnitDots
{
   

    public class UnitUpdateDto : UnitCreateDto
    {
        public int UnitId { get; set; }
        public int Head_Of_UnitId { get; set; } // Assuming the Head_Of_Unit is an Employee ID
        public List<int> EmployeeIds { get; set; } = new List<int>(); // List of Employee IDs
    }
}
