using FinalProject.Core.Models;
using FinalProject.Core.Models;
using System.Collections.Generic;

namespace FinalProject.Core.IRepositories
{
    public interface IUnitRepository : IBaseRepository<Unit>
    {

        public Task<Employee> AddEmployeeAsync(int UnitId, int EmployeeId);
        public Task<ICollection<Employee>> AddEmployeeRangeAsync(int UnitId, int[] EmployeesId);

        public Task<Unit> RemoveEmployeeAsync(int UnitId, int EmployeeId);

        //Unit GetUnitById(int id, string[] includeProperties = null);
        //IEnumerable<Unit> GetAllUnits(string[] includeProperties = null);
    }
}
