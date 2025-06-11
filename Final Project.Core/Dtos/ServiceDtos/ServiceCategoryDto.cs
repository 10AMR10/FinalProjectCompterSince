using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Core.Dtos.ServiceDtos
{
	public class ServiceCategoryDto
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string PdfDescription { get; set; }
		public int CategoryId { get; set; }
		public string CategoryName { get; set; }
	}
}
