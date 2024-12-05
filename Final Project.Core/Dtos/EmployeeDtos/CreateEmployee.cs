using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Core.Dtos.EmployeeDots
{
    public class EmployeeCreateDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Job_Title { get; set; }
        public IFormFile Resume { get; set; }
        //public int DepartmentId { get; set; }
        //public int? UnitId { get; set; }
    }

   
}
