using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FinalProject.Core.Models
{
	public class Course
	{
		public int CourseId { get; set; }
		public string Title { get; set; }
		public string ArabicTitle { get; set; }

		//public string Description { get; set; }
		public string LevelYear { get; set; }
		public string ArabicLevelYear { get; set; }
		public string PdfDescription { get; set; }

		public int? DepartmentId { get; set; }
		
		public Department? Department { get; set; }
	}
}
