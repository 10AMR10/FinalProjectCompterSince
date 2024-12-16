using FinalProject.Core.Models.identity;
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
		public static async Task SeedingIdentityAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			if (userManager.Users.Count() == 0)
			{
				await roleManager.CreateAsync(new IdentityRole("Admin"));
				await roleManager.CreateAsync(new IdentityRole("Doctor"));

				var user = new ApplicationUser()
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
