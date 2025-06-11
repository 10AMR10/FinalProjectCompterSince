using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Core.Models
{
	public class Service
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string ArabicTitle { get; set; }
		public string PdfDescription { get; set; }
        public DateTime Date { get; set; }= DateTime.Now;
        public Category? category { get; set; }
        public int? categoryId { get; set; }

    }
}
