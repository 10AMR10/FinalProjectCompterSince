
using FinalProject.Core.IRepositories;
using FinalProject.Core.IRepositories;
using FinalProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProject;


namespace FinalProject.Core 
{
    public interface IUnitOfWork : IDisposable
    {
        public IEventRepository Events { get; }
        public INewsRepository News { get; }
        public IDepartmentRepository Departments { get; }
        public IEmployeeRepository Employees { get; }
        public ICourseRepository Courses { get; }
       
        public IUnitRepository Units { get; }
        
        public IQualityRepository Qualities { get; }

        int Complete();
        Task<int> CompleteAsync();
    }
}

