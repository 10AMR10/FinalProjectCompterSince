using FinalProject.Core.Models;
using FinalProject.EF.Configuration;
using FinalProject.Core.IRepositories;
using FinalProject.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FinalProject.EF.RepositoriesImplementation
{
	public class EventRepositoryImp : BaseRepositoryImp<Event>, IEventRepository
	{
		//public EventRepositoryImp(ApplicationDbContext context) : base(context)
		//{
		//}
		private readonly ApplicationDbContext _context;
		public EventRepositoryImp(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		

		

	}
}
