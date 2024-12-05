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

namespace FinalProject.EF.RepositoriesImplementation
{
    public class UnitRepositoryImp : BaseRepositoryImp<Unit>, IUnitRepository
    {
        private readonly ApplicationDbContext _context;
        public UnitRepositoryImp(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        

        public async Task<Employee> AddEmployeeAsync(int UnitId, int EmployeeId)
        {
            var Unit = await _context.Units.FindAsync(UnitId);
            var employee = await _context.Employees.FindAsync(EmployeeId);
            //Unit.Employees.Append(employee);
            employee.Unit = Unit;
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Unit> RemoveEmployeeAsync(int UnitId, int EmployeeId)
        {
            var Unit = await _context.Units.FindAsync(UnitId);
            var employee = await _context.Employees.FindAsync(EmployeeId);
            Unit.Employees.Remove(employee);

            _context.Units.Update(Unit);
            await _context.SaveChangesAsync();
            return Unit;
        }

        public async Task<ICollection<Employee>> AddEmployeeRangeAsync(int id, ICollection<Employee> employees)
        {
            var unit = await _context.Departments.FindAsync(id);

            foreach (var employee in employees)
            {
                unit.Employees.Append(employee);
            }
            await _context.SaveChangesAsync();
            return employees;
        }

        public Task<ICollection<Employee>> AddEmployeeRangeAsync(int UnitId, int[] EmployeesId)
        {
            throw new NotImplementedException();
        }
    }
}
