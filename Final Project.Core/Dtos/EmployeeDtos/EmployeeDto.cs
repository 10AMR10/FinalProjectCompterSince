using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Core.Dtos.EmployeeDtos
{
	public class EmployeeDto
	{
		public int EmployeeId { get; set; }
		public string Name { get; set; }
		
		public string Image { get; set; }
		public string Email { get; set; }

		public string Job_Title { get; set; }
		public string Resume { get; set; }

		//[ForeignKey("Department")]
		public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }

    }
}
