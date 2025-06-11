namespace FinalProject.Core.Models
{
	public class Course
	{
		public int CourseId { get; set; }
		public string Title { get; set; }
		public string ArabicTitle { get; set; }

		//public string Description { get; set; }
		public LevelYear? levelYear { get; set; }
        public int? LevelYearId { get; set; }
        //public string ArabicLevelYear { get; set; }
		public string PdfDescription { get; set; }

		public int? DepartmentId { get; set; }
		
		public Department? Department { get; set; }
	}
}
