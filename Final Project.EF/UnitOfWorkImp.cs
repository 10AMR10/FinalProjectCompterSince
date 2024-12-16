using FinalProject.Core;
using FinalProject.Core.IRepositories;
using FinalProject.EF.Configuration;
using FinalProject.EF.RepositoriesImplementation;

namespace FinalProject.EF
{
	public class UnitOfWorkImp : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWorkImp(ApplicationDbContext context)
        {
             _context = context;
            Events = new EventRepositoryImp(_context);
            News = new NewsRepositoryImp(_context);
            Departments = new DepartmentRepositoryImp(_context);
            Employees = new EmployeeRepositoryImp(_context);
            Courses = new CourseRepositoryImp(_context);
            
            Units = new UnitRepositoryImp(_context);
             
            Qualities = new QualityRepositoryImp(_context);
            UnitEmployees = new UnitEmployeesRepositryImp(_context);
			UnitCourses=new UnitCoursesRepositryImp(_context);

		}

        public IEventRepository Events { get; private set; }
        public INewsRepository News { get; private set; }
        public IDepartmentRepository Departments { get; private set; }
        public IEmployeeRepository Employees { get; private set; }
        public ICourseRepository Courses { get; private set; }
       
        public IUnitRepository Units { get; private set; }
        
        public IQualityRepository Qualities { get; private set; }
		public IUnitEmployeesRepository UnitEmployees { get; private set; }
		public IUnitCoursesRepositry UnitCourses { get; private set; }


		public int Complete()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}


