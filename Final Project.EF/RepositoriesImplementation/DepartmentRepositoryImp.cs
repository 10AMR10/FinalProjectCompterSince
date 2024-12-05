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
    public class DepartmentRepositoryImp : BaseRepositoryImp<Department>, IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;
        public DepartmentRepositoryImp(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Employee> AddEmployeeAsync(int DepartmentId,int EmployeeId)
        {
            var department = await _context.Departments.FindAsync(DepartmentId);
            var employee = await _context.Employees.FindAsync(EmployeeId);
            if (department == null || employee == null) return null;
            employee.Department = department;
             //department.Employees.Append(employee);
            await _context.SaveChangesAsync();
            return employee;    
        }

        public async Task<Department> RemoveEmployeeAsync(int DepartmentId, int EmployeeId)
        {
            var Department = await _context.Departments.FindAsync(DepartmentId);
            var employee = await _context.Employees.FindAsync(EmployeeId);
            Department.Employees.Remove(employee);

            _context.Departments.Update(Department);
            await _context.SaveChangesAsync();
            return Department;
        }




        public async Task<ICollection<Employee>> AddEmployeeRangeAsync( int id,ICollection<Employee> employees)
        {
            var department = await _context.Departments.FindAsync(id);

            foreach (var employee in employees)
            {
                department.Employees.Append(employee);
            }
            await _context.SaveChangesAsync();
            return employees;
        }

        public async Task<ICollection<Employee>> AddEmployeeRangeAsync(int DepartmentId, int[] EmployeeId)
        {
            var department = await _context.Departments.FindAsync(DepartmentId);
            var employee = await _context.Employees.AnyAsync();

            //foreach (var employee in employees)
            //{
            //    department.Employees.Append(employee);
            //}
            await _context.SaveChangesAsync();
            return null;
        }
    }
}