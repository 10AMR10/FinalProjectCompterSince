using FinalProject.Core.Models;
using FinalProject.Core;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using FinalProject.Core.Dtos.EmployeeDots;
using FinalProject.Core.Models;
using Microsoft.AspNetCore.Http;
using FinalProject.Core.Dtos.CollegeDots;
using System.Collections.Generic;
using FinalProject.EF;
using FinalProject.Core;
using FinalProject.EF.Migrations;


namespace FinalProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmployeeController(IUnitOfWork unitOfWork )
        {
            _unitOfWork = unitOfWork;
        }

        // POST: api/Employee/Create_Employee
        [HttpPost("/Create_Employee")]
        public async Task<IActionResult> Create(EmployeeCreateDto employeeDto)
        {
            var employee = new Employee()
            {
                Name = employeeDto.Name,
                Email = employeeDto.Email,
                Password = employeeDto.Password,
                Job_Title = employeeDto.Job_Title,
                //Resume =  _unitOfWork.Employees.UploadEmployeeCV(employeeDto.Resume , null).ToString(),
                //DepartmentId = employeeDto.DepartmentId
                //Department =await _unitOfWork.Departments.GetByIdAsync(d => d.DepartmentId == employeeDto.DepartmentId)
            };
            employee.Resume = _unitOfWork.Employees.UploadEmployeeCV(employeeDto.Resume, employee);
            var AddEmployee = await _unitOfWork.Employees.AddAsync(employee);
            if (AddEmployee == null) return BadRequest("Employee creation failed");

            await _unitOfWork.CompleteAsync();
            return Ok(employee);
        }

        [HttpPost("/Upload_CV")]
        public async Task<IActionResult> UploadCV( int employeeId , IFormFile CVFile)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(e => e.EmployeeId == employeeId);
            if (employee == null) return NotFound("Employee not found");

            //var uploadDirectory = _hostEnvironment.WebRootPath ??
            //    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" ,"uploads") ;

            //Directory.CreateDirectory(uploadDirectory);

            if( _unitOfWork.Employees.UploadEmployeeCV( CVFile , employeeId) == null)
                return BadRequest("CV Upload failed");

            await _unitOfWork.CompleteAsync();

            return Ok("CV Uploaded Successfully");
            
            
                
        }

        // GET: api/Employee/Get_Employee_By_Id/{id}
        [HttpGet("/Get_Employee_By_Id/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Employee employee =await _unitOfWork.Employees.GetByIdAsync(e => e.EmployeeId == id, new[] { "Department" , "Unit" });

            if (employee == null)
                return NotFound("Epmloyee not found");

            return Ok(employee);
        }

        // GET: api/Employee/Get_All_Employees
        [HttpGet("/Get_All_Employees")]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Employee> employees =await _unitOfWork.Employees.GetAllAsync(null,new[] { "Department" , "Unit" });

            if (employees == null) return NotFound("There is no employee added yet.");

            return Ok(employees);
        }

        // PUT: api/Employee/Update_Employee
        [HttpPut("/Update_Employee")]
        public async Task<IActionResult> Update(EmployeeUpdateDto employeeDto)
        {
            Employee employee =await _unitOfWork.Employees.GetByIdAsync(e => e.EmployeeId == employeeDto.EmployeeId, new[] { "Department" });
            if (employee == null) return NotFound("Employee not found");

            employee = new Employee()
            {
                EmployeeId = employeeDto.EmployeeId,
                Name = employeeDto.Name,
                Email = employeeDto.Email,
                Password = employeeDto.Password,
                Job_Title = employeeDto.Job_Title,
                Resume = _unitOfWork.Employees.UploadEmployeeCV(employeeDto.Resume , employeeDto.EmployeeId),
                //DepartmentId = employeeDto.DepartmentId,
                Department = await _unitOfWork.Departments.GetByIdAsync(d=> d.DepartmentId == employeeDto.DepartmentId)
            };


            

            var updatedEmployee =await _unitOfWork.Employees.UpdateAsync(employee);
            if(updatedEmployee == null) return BadRequest("Employee update operation failed");

            await _unitOfWork.CompleteAsync();
            return Ok(employee);
        }

        // DELETE: api/Employee/Delete_Employee/{id}
        [HttpDelete("/Delete_Employee/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Employee employee =await _unitOfWork.Employees.GetByIdAsync(e => e.EmployeeId == id, new[] { "Department" });

            if (employee == null)
                return NotFound("Employee Not Found");

            var DeleteEmployee = await _unitOfWork.Employees.DeleteAsync(id);
            if (DeleteEmployee == null) return BadRequest("Delete Employee operation failed");
            await _unitOfWork.CompleteAsync();
            return Ok(employee);
        }
    }
}
