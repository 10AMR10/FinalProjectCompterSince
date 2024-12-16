using FinalProject.Core.Models.identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FinalProject.Core.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string ArabicName { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
     
        public string ArabicJob_Title { get; set; }
        public string Job_Title { get; set; }
		public string Resume { get; set; }

        //[ForeignKey("Department")]
        public int DepartmentId { get; set; }
        
        public Department? Department { get; set; }

        //public int UnitId { get; set; }
 
        public ApplicationUser? applicationUser { get; set; }
    }
}
