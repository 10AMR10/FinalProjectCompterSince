using FinalProject.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Core.Dtos.UnitDots
{
    public class AddEmployeeToUnitDto
    {
		
		public string Name { get; set; }
		public string ArabicName { get; set; }

		public string ArabicJob_Title { get; set; }
		public string Job_Title { get; set; }
		public IFormFile Resume { get; set; }
		
		

	}
}
