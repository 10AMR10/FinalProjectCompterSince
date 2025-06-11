using Microsoft.AspNetCore.Http;

namespace FinalProject.Core.Dtos.CourseDtos
{
	public class CreateCourseDto
    {
		//public int CourseId { get; set; }
		public string Title { get; set; }
		
		public int LevelYearId { get; set; }
		
		public IFormFile PdfDescription { get; set; }
		public int? DepartmentId { get; set; }
	}

}
