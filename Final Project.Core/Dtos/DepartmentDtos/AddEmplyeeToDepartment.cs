using FinalProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Core.Dtos.DepartmentDtos
{
    public class AddEmplyeeToDepartment
    {
        public int DepartmentId { get; set; }

        public int EmployeeId { get; set; }
    }
}
