using FinalProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Core.Models
{
    public class Event
    {//dev
        public int EventId { get; set; }
        public string Name { get; set; }
        public string ArabicName { get; set; }
		public string ArabicDescription { get; set; }
		public string Description { get; set; }
		public string img { get; set; }
        
        public DateTime Event_Start_Date { get; set; }


	}
}
