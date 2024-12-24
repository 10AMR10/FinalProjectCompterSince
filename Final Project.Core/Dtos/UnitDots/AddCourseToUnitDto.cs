using Microsoft.AspNetCore.Http;

namespace FinalProject.Core.Dtos.UnitDots
{
	public class AddCourseToUnitDto
	{
		public string Title { get; set; }
		public string ArabicTitle { get; set; }

		//public string Description { get; set; }

		public IFormFile? PdfDescription { get; set; }
		
	}
}
