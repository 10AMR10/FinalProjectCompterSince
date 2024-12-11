using Microsoft.AspNetCore.Identity;

namespace DiabetesApp.Core.Service.Contract
{

	public interface ITokentService
		{
			Task<string> CreateTokenAsync(IdentityUser user, UserManager<IdentityUser> userManager);
		}
	
}
