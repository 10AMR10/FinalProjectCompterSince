using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Core.Dtos.UnitDots
{
	public class UnitCoursesToRetrunDto
	{
		public int Id { get; set; }
		public string Title { get; set; }


		//public string Description { get; set; }

		public string PdfDescription { get; set; }
        public int? UnitId { get; set; }
        public string? UnitName { get; set; }
    }
}
