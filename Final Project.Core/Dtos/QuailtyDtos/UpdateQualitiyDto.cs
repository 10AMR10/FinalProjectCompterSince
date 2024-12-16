using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Core.Dtos.QuailtyDtos
{
	public class UpdateQualitiyDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string ArabicName { get; set; }
		public string ArabicDescription { get; set; }
		public string Description { get; set; }
	}
}
