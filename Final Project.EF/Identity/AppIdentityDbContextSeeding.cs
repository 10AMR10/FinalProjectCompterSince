using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.EF.Identity
{
	public static class AppIdentityDbContextSeeding
	{
		public static async Task SeedingIdentityAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			if (userManager.Users.Count() == 0)
			{
				await roleManager.CreateAsync(new IdentityRole("Admin"));
				await roleManager.CreateAsync(new IdentityRole("Doctor"));

				var user = new IdentityUser()
				{
					Email = "Admin@gmail.com",
					UserName = "Admin"
				};
				await userManager.CreateAsync(user, "Admin123");
				await userManager.AddToRoleAsync(user, "Admin");
				//StrongP@ssw0rd
			}
		}
	}
}
