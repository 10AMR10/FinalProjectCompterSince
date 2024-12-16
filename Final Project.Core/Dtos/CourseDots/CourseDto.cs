using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Core.Dtos.CourseDots
{
	public class CourseDto
	{
		public int CourseId { get; set; }
		public string Title { get; set; }
		public string LevelYear { get; set; }
		public string PdfDescription { get; set; }
		public string? DepartmentName { get; set; }

	}
}
