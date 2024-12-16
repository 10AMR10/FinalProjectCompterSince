using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Core.Dtos.EventDtos
{
    public class CreateEvent
    {
        public string Name { get; set; }
        public string ArabicName { get; set; }
		public string Description { get; set; }
		public string ArabicDescription { get; set; }
		public DateTime Event_Start_Date { get; set; }
        public IFormFile  Image { get; set; }
    
    }
}
