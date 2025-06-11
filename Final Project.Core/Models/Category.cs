using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Core.Models
{
	public class Category
	{
        public int Id { get; set; }
		public string Name { get; set; }
		public string ArabicName { get; set; }
		public ICollection<Service>? Services { get; set; } = new HashSet<Service>();
    }
}
