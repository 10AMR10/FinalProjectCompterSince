using Microsoft.AspNetCore.Http;

namespace FinalProject.Core.Dtos.NewsDtos
{
	public class CreateNewsDto
    {
        public string Name { get; set; }
  //      public string ArabicName { get; set; }
		//public string ArabicDescription { get; set; }
		public string Description { get; set; }
		public DateTime News_Date { get; set; }
		public IFormFile? Image { get; set; }

		

    }
}
