using FinalProject.Core.Models.identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Core.Models
{
	public class UnitEmployees
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string ArabicName { get; set; }
	
		public string ArabicJob_Title { get; set; }
		public string Job_Title { get; set; }
		public string Resume { get; set; }
        public int? UnitId { get; set; }
        public Unit? Unit { get; set; }


    }
}
