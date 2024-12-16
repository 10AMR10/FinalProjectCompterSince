using FinalProject.Core.Models.identity;
using Microsoft.AspNetCore.Identity;

namespace DiabetesApp.Core.Service.Contract
{

	public interface ITokentService
		{
			Task<string> CreateTokenAsync(ApplicationUser user, UserManager<ApplicationUser> userManager);
		}
	
}
