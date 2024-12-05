using FinalProject.Core.Dtos.DepartmentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Core.Dtos.DepartmentDots
{
	public class UpdateDepartmentDto : CreateDepartmentDto
	{
		public int DepartmentId { get; set; }
        public int HeadOfDepartmentId { get; set; }

    }
}
