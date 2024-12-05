using FinalProject.Core.Dtos.DepartmentDtos;
using FinalProject.Core.Models;
using FinalProject.Core;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using FinalProject.Core.Models;
using FinalProject.Core.Dtos.DepartmentDots;
using FinalProject.Core.Dtos.UnitDots;

using FinalProject.Core.Dtos.CollegeDots;
using FinalProject.EF.Migrations;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using FinalProject.EF;
using FinalProject.Core;
namespace FinalProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // POST: api/Department/Create_Department
        [HttpPost("/Create_Department")]
        public async Task<IActionResult> Create(CreateDepartmentDto departmentDto)
        {
            Department department = new Department()
            {
                Name = departmentDto.Name,
                Description = departmentDto.Description,
                
                Head_Of_Department = await _unitOfWork.Employees.GetByIdAsync(e => e.EmployeeId == departmentDto.HeadOfDepartmentId),

            };

            var AddDepartment = await _unitOfWork.Departments.AddAsync(department);
            if ( AddDepartment == null) return BadRequest("Add Department operation failed");

            await _unitOfWork.CompleteAsync();
            return Ok(department);
        }

        // GET: api/Department/Get_Department_By_Id/{id}
        [HttpGet("/Get_Department_By_Id/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Department department =await _unitOfWork.Departments.GetByIdAsync(d => d.DepartmentId == id, new[] { "College", "Head_Of_Department", "Employees", "Courses" });
            if (department == null) return NotFound("Department not found");

            return Ok(department);
        }

        // GET: api/Department/Get_All_Departments
        [HttpGet("/Get_All_Departments")]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Department> departments =await _unitOfWork.Departments.GetAllAsync(null,new[] { "College" , "Head_Of_Department" , "Employees" , "Courses" });
            if (departments == null) return NotFound("There is no department created yet");

            return Ok(departments);
        }

        // PUT: api/Department/Update_Department
        [HttpPut("/Update_Department")]
        public async Task<IActionResult> Update(UpdateDepartmentDto departmentDto)
        {
            var department =await _unitOfWork.Departments.GetByIdAsync(d => d.DepartmentId == departmentDto.DepartmentId, new[] { "College" });

            if ( department == null)
                return NotFound("Department not found");

             department = new Department()
            {
                DepartmentId = departmentDto.DepartmentId,
                Name = departmentDto.Name,
                Description = departmentDto.Description,

                
                //College = await _unitOfWork.Colleges.GetByIdAsync(c => c.CollegeId == departmentDto.CollegeId),

                //Head_Of_DepartmentEmployeeId = departmentDto.HeadOfDepartmentId,
                //Head_Of_Department = await _unitOfWork.Employees.GetByIdAsync(e => e.EmployeeId == departmentDto.HeadOfDepartmentId),
            };

            var UpdateDepartment = await _unitOfWork.Departments.UpdateAsync(department);
            if (UpdateDepartment == null) return BadRequest("Update department operation failed.");
            await _unitOfWork.CompleteAsync();
            return Ok(department);
        }

        [HttpPut("/Add_Emloyee_To_Departmet")]
        public async Task<IActionResult> AddEmployeeToDepartment(AddEmplyeeToDepartment AddEmployeeDto)
        {


            var employee = await _unitOfWork.Employees.GetByIdAsync(e => e.EmployeeId == AddEmployeeDto.EmployeeId);
            if (employee == null) return NotFound("Employee not found");

            var Department = await _unitOfWork.Departments.GetByIdAsync(d=> d.DepartmentId == AddEmployeeDto.DepartmentId);
            if (Department == null) return NotFound("Department not found");

            var employeeAdded = await _unitOfWork.Departments.AddEmployeeAsync(AddEmployeeDto.DepartmentId, AddEmployeeDto.EmployeeId);

            if(employeeAdded ==null) return NotFound();

            await _unitOfWork.CompleteAsync();
            return Ok(employeeAdded);

        }


        [HttpPut("/Remove_Emloyee_From_Department")]
        public async Task<IActionResult> RemoveEmployeeFromDepartment(AddEmplyeeToDepartment AddEmployeeDto)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(e => e.EmployeeId == AddEmployeeDto.EmployeeId);
            if (employee == null) return NotFound("Employee not found");

            var Department = await _unitOfWork.Departments.GetByIdAsync(d => d.DepartmentId == AddEmployeeDto.DepartmentId);
            if (Department == null) return NotFound("Department not found");


            var employeeRemoved = await _unitOfWork.Departments.RemoveEmployeeAsync(AddEmployeeDto.DepartmentId, AddEmployeeDto.EmployeeId);

            if (employeeRemoved == null) return NotFound();
            await _unitOfWork.CompleteAsync();

            return Ok(employeeRemoved);

        }

        // DELETE: api/Department/Delete_Department/{id}
        [HttpDelete("/Delete_Department/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Department department =await _unitOfWork.Departments.GetByIdAsync(d => d.DepartmentId == id, new[] { "College" });
            if (department == null) return NotFound("Department Not Found");

            var DeleteDepartment = await _unitOfWork.Departments.DeleteAsync(id);
            if (DeleteDepartment == null) return BadRequest("Delete department operation failed");
            await _unitOfWork.CompleteAsync();
            return Ok(department);
        }
    }
}
