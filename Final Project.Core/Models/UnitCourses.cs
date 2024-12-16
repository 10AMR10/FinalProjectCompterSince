using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FinalProject.Core.Models
{
	public class UnitCourses
	{
		
		public int Id { get; set; }
		public string Title { get; set; }
		public string ArabicTitle { get; set; }

		//public string Description { get; set; }

		public string PdfDescription { get; set; }
        public Unit? unit { get; set; }
        public int UnitId { get; set; }


        
	}
}
