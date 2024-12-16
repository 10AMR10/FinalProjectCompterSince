using FinalProject.Core.IRepositories;
using FinalProject.Core.Models;
using FinalProject.EF.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.EF.RepositoriesImplementation
{
	public class UnitEmployeesRepositryImp: BaseRepositoryImp<UnitEmployees>, IUnitEmployeesRepository
	{
		private readonly ApplicationDbContext _context;
		public UnitEmployeesRepositryImp(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}
	}
}
