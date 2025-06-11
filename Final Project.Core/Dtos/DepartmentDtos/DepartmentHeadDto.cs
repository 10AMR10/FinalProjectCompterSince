using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Core.Dtos.DepartmentDtos
{
	public class DepartmentHeadDto
	{
		public int Id { get; set; }
		public string Description { get; set; }
		public string Name { get; set; }
		public int? EmployeeId { get; set; }
		public string? EmpName { get; set; }
		
	}
}
