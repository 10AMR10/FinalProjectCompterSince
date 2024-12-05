using FinalProject.Core.Models;
using FinalProject.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Core.IRepositories
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {

        public string UploadEmployeeCV(IFormFile CvFile , int? employeeId);
        public string UploadEmployeeCV(IFormFile CvFile, Employee? employee);

        //Task<IEnumerable<Employee>> GetAllAsync();
        //Task<Employee> GetByIdAsync(int id);
        //Task<bool> AddAsync(Employee employee);
        //Task<bool> UpdateAsync(Employee employee);
        //Task<bool> DeleteAsync(int id);
    }
}
