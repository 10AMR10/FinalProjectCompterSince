using FinalProject.Core.Models;
using FinalProject.EF.Configuration;
using FinalProject.Core.IRepositories;
using FinalProject.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.EF.RepositoriesImplementation
{
    public class EmployeeRepositoryImp : BaseRepositoryImp<Employee>, IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;
        public EmployeeRepositoryImp(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

       

        //public async Task<string> UploadEmployeeCVAsync(int employeeId , IFormFile CVFile , string uploadDirectory)
        //{
        //    if (CVFile == null || CVFile.Length == 0) throw new ArgumentException("Invalid CV file");
        //    var employee = await _context.Employees.FindAsync(employeeId);
        //    if (employee == null) throw new KeyNotFoundException("Employee Not found");

        //    var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(CVFile.FileName)}";

        //    var filePath = Path.Combine(uploadDirectory, fileName);

        //    using(var stream = new FileStream(filePath , FileMode.Create))
        //    {
        //        await CVFile.CopyToAsync(stream);
        //    }

        //    employee.Resume = filePath;

        //     _context.Employees.Update(employee);
        //    await _context.SaveChangesAsync();
        //    return employee.Resume;
        //}


    }
}
