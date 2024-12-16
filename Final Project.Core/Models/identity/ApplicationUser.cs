using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Core.Models.identity
{
	public class ApplicationUser : IdentityUser
	{
        // navigitional property with hospital

        public Employee? employee { get; set; }
        public int? EmployeId { get; set; }
    }
}
