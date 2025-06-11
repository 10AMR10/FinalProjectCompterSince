using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Core.Dtos.UnitDots
{
	public class UnitEmployeeToReturnDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Job_Title { get; set; }
		public string Resume { get; set; }
		public string? UnitName { get; set; }
        public int? UnitId { get; set; }
    }
}
