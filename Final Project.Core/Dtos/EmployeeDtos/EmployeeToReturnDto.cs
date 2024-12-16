using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Core.Dtos.EmployeeDtos
{
	public class EmployeeToReturnDto
	{
		public int EmployeeId { get; set; }
		public string Name { get; set; }
		public string Job_Title { get; set; }
        public string  DepartmentName { get; set; }
		public string Resume { get; set; }

	}
}
