using FinalProject.Core.Models;
using FinalProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Core.IRepositories
{
    public interface IDepartmentRepository : IBaseRepository<Department>
    {

        public Task<Employee> AddEmployeeAsync(int DepartmentId , int EmployeeId);
        public Task<ICollection<Employee>> AddEmployeeRangeAsync(int DepartmentId,int[] EmployeeId);
        public Task<Department> RemoveEmployeeAsync(int DepartmentId, int EmployeeId);
        //Task<IEnumerable<Department>> GetAllAsync();
        //Task<Department> GetByIdAsync(int id);
        //Task<bool> AddAsync(Department department);
        //Task<bool> UpdateAsync(Department department);
        //Task<bool> DeleteAsync(int id);
    }
}
