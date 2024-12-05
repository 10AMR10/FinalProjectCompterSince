using FinalProject.Core.Models;
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
    public class CourseRepositoryImp : BaseRepositoryImp<Course>, ICourseRepository
    {
        public CourseRepositoryImp(ApplicationDbContext context) : base(context)
        {

        }


    }
}