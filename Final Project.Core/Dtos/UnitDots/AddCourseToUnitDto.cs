using FinalProject.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Core.Dtos.UnitDots
{
	public class AddCourseToUnitDto
	{
		public string Title { get; set; }
		public string ArabicTitle { get; set; }

		//public string Description { get; set; }

		public IFormFile PdfDescription { get; set; }
		
	}
}
