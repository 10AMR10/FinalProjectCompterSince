using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FinalProject.Core.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        

        //[ForeignKey ("Head_Of_Department")]
        //public int Head_Of_DepartmentEmployeeId { get; set; }
        public Employee? Head_Of_Department { get; set; }

        public ICollection<Employee>? Employees { get; set; }

        public ICollection<Course>? Courses { get; set; }
    }
}
