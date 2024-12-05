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

        public  string UploadEmployeeCV( IFormFile CvFile , int? employeeId)
        {

            var employee =  _context.Employees.Find(employeeId);
            //if (employee == null) return null;
            _context.Entry(employee).State = EntityState.Detached;

            var validExtentions = new List<string>() { ".pdf" , ".docx"};
            var extention = Path.GetExtension(CvFile.FileName);
            if (!validExtentions.Contains(extention)) return $"Extention is not valid {string.Join(',',validExtentions)}";

            long size = CvFile.Length;
            if (size > (5 * 1024 * 1024))
                return $"Maximum size can be 5mb";

            //var filename = Guid.NewGuid().ToString() + extention;
            var filename = Guid.NewGuid() + Path.GetFileName(CvFile.FileName);

            var path = Path.Combine(Directory.GetCurrentDirectory(),"Uploads");
            FileStream stream = new FileStream(Path.Combine(path , filename), FileMode.Create);

            CvFile.CopyTo(stream);

            employee.Resume = filename;
             _context.SaveChanges();

            return employee.Resume;

        }

        public  string UploadEmployeeCV(IFormFile CvFile, Employee? employee)
        {
            //var employee = await _context.Employees.FindAsync(employeeId);
            //if (employee == null) return null;

            var validExtentions = new List<string>() { ".pdf", ".docx" };
            var extention = Path.GetExtension(CvFile.FileName);
            if (!validExtentions.Contains(extention)) return $"Extention is not valid {string.Join(',', validExtentions)}";

            long size = CvFile.Length;
            if (size > (5 * 1024 * 1024))
                return $"Maximum size can be 5mb";

            //var filename = Guid.NewGuid().ToString() + extention;
            var filename = Guid.NewGuid() + Path.GetFileName(CvFile.FileName);

            var path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            FileStream stream = new FileStream(Path.Combine(path, filename), FileMode.Create);

            CvFile.CopyTo(stream);

            employee.Resume = filename;
             _context.SaveChanges();

            return employee.Resume;
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
