using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Core.Dtos.DepartmentDtos
{
	public class DepartmentDto
	{
		public string Description { get; set; }
		public string Name { get; set; }
		public int EmployeeId { get; set; }
		public string EmpName { get; set; }
		public string EmpJob_Title { get; set; }
		public string EmpResume { get; set; }
		public string EmpImage { get; set; }
        public int EmpId { get; set; }

    }
}
