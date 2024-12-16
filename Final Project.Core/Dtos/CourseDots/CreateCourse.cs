using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Core.Dtos.CourseDtos
{
    public class CreateCourseDto
    {
		//public int CourseId { get; set; }
		public string Title { get; set; }
		public string ArabicTitle { get; set; }
		public string LevelYear { get; set; }
		public string ArabicLevelYear { get; set; }
		public IFormFile PdfDescription { get; set; }
		public int? DepartmentId { get; set; }
	}

}
