using FinalProject.EF.Configuration;
using FinalProject.Core.IRepositories;
using FinalProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.EF.RepositoriesImplementation
{
    public class NewsRepositoryImp : BaseRepositoryImp<News> , INewsRepository
    {
        public NewsRepositoryImp(ApplicationDbContext context) : base(context) {
        
        }


    }
}
