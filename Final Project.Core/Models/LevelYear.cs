using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Core.Models
{
	public class LevelYear
	{
        public int Id { get; set; }
		public string Name { get; set; }
		public string ArabicName { get; set; }
        public ICollection<Course>? Courses { get; set; }=new HashSet<Course>();
    }
}
