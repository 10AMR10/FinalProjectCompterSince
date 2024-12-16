using FinalProject.Core;
using FinalProject.Core.Dtos.DepartmentDots;
using FinalProject.Core.Dtos.DepartmentDtos;
using FinalProject.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
		[Authorize(Roles = "Admin")]
		[HttpPost("/Create_Department")]
        public async Task<ActionResult<bool>> Create(CreateDepartmentDto departmentDto)
        {
            var department = new Department()
            {
                Name = departmentDto.Name,
                ArabicName=departmentDto.ArabicName,
                Description = departmentDto.Description,
                ArabicDescription=departmentDto.ArabicDescription,
                Head_Of_Department = await _unitOfWork.Employees.GetByIdAsync(e => e.EmployeeId == departmentDto.HeadOfDepartmentId),

            };

            var AddDepartment = await _unitOfWork.Departments.AddAsync(department);
            if ( AddDepartment == null) return BadRequest("Add Department operation failed");

			int res = await _unitOfWork.CompleteAsync();
			if (res > 0)
				return Ok(department);
			return BadRequest("Department Create operation failed");
		}

        // GET: api/Department/Get_Department_By_Id/{id}
        [HttpGet("/Get_Department_By_Id/{id}/{lang}")]
        public async Task<ActionResult<DepartmentDto>> Get(int id,string lang)
        {
            Department department = await _unitOfWork.Departments.GetByIdAsync(d => d.DepartmentId == id, new[] { "Head_Of_Department" });
            if (department == null) 
                return NotFound("Department not found");
            if (lang=="eng")
            {

                var mapped = new DepartmentDto
                {
                    Description = department.Description,
                    Name = department.Name,
                    EmpImage = department.Head_Of_Department.Image,
                    EmpJob_Title = department.Head_Of_Department.Job_Title,
                    EmployeeId = department.Head_Of_Department.EmployeeId,
                    EmpName = department.Head_Of_Department.Name,
                    EmpResume = department.Head_Of_Department.Resume,
                    EmpId=department.Head_Of_Department.EmployeeId
                };
                return Ok(mapped);
            }
            else
            {
				var mapped = new DepartmentDto
				{
					Description = department.ArabicDescription,
					Name = department.ArabicName,
					EmpImage = department.Head_Of_Department.Image,
					EmpJob_Title = department.Head_Of_Department.ArabicJob_Title,
					EmployeeId = department.Head_Of_Department.EmployeeId,
					EmpName = department.Head_Of_Department.ArabicName,
					EmpResume = department.Head_Of_Department.Resume
				};
				return Ok(mapped);
			}
        }
        //GET: api/Department/GetDetails
        [HttpGet]

        // GET: api/Department/Get_All_Departments
        [HttpGet("/Get_All_Departments/{lang}")]
        public async Task<ActionResult<IEnumerable<Department>>> GetAll(string lang)
        {
            IEnumerable<Department> departments = await _unitOfWork.Departments.GetAllAsync(null,null);
            if (departments == null) return NotFound("There is no department created yet");
            if (lang=="eng")
            {
                
                return Ok(departments.Select(x=> new Department
				{
                    DepartmentId=x.DepartmentId,
                    Name=x.Name,
                }));
            }
            else
            {
				
				return Ok(departments.Select(x => new Department
				{
					DepartmentId = x.DepartmentId,
					Name = x.ArabicName,
				}));
			}
        }

		// PUT: api/Department/Update_Department
		[Authorize(Roles = "Admin")]
		[HttpPut("/Update_Department/{id}")]
        public async Task<ActionResult<Department>> Update(int id,[FromBody] CreateDepartmentDto departmentDto)
        {
            var department =await _unitOfWork.Departments.GetByIdAsync(d => d.DepartmentId == id);

            if ( department == null)
                return NotFound("Department not found");


            department.Name = departmentDto.Name;
            department.ArabicName = departmentDto.ArabicName;
            department.Description = departmentDto.Description;
            department.ArabicDescription = departmentDto.ArabicDescription;
            department.Head_Of_Department = await _unitOfWork.Employees.GetByIdAsync(e => e.EmployeeId == departmentDto.HeadOfDepartmentId);

		
			_unitOfWork.Departments.Update(department);
            
			int res = await _unitOfWork.CompleteAsync();
			if (res > 0)
				return Ok(department);
			return BadRequest("Department Update operation failed");
		}
		[Authorize(Roles = "Admin")]
		[HttpPut("/Add_Emloyee_To_Departmet/{departmentId}/{employeeId}")]
        public async Task<ActionResult<bool?>> AddEmployeeToDepartment(int departmentId,int employeeId)
        {

            var employee = await _unitOfWork.Employees.GetByIdAsync(e => e.EmployeeId == employeeId);
            if (employee == null) return NotFound("Employee not found");

            var department = await _unitOfWork.Departments.GetByIdAsync(d=> d.DepartmentId == departmentId);
            if (department == null) return NotFound("Department not found");

            department.Employees.Add(employee);
			_unitOfWork.Departments.Update(department);

			int res = await _unitOfWork.CompleteAsync();
			if (res > 0)
				return Ok(true);
			return BadRequest("Employee added  failed");
			
        }

		[Authorize(Roles = "Admin")]
		[HttpPut("/Remove_Emloyee_From_Department")]
        public async Task<ActionResult<bool>> RemoveEmployeeFromDepartment(AddEmplyeeToDepartment AddEmployeeDto)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(e => e.EmployeeId == AddEmployeeDto.EmployeeId);
            if (employee == null) return NotFound("Employee not found");

            var department = await _unitOfWork.Departments.GetByIdAsync(d => d.DepartmentId == AddEmployeeDto.DepartmentId);
            if (department == null) return NotFound("Department not found");

            department.Employees.Remove(employee);
            _unitOfWork.Departments.Update(department);
			int res = await _unitOfWork.CompleteAsync();
			if (res > 0)
				return Ok(true);
			return BadRequest("Employee Removed  failed");

        }

		// DELETE: api/Department/Delete_Department/{id}
		[Authorize(Roles = "Admin")]
		[HttpDelete("/Delete_Department/{id}")]
        public async Task<ActionResult<Department>> Delete(int id)
        {
            Department department =await _unitOfWork.Departments.GetByIdAsync(d => d.DepartmentId == id, new[] { "College" });
            if (department == null) return NotFound("Department Not Found");

            _unitOfWork.Departments.Delete(department);
            
			int res = await _unitOfWork.CompleteAsync();
			if (res > 0)
				return Ok(department);
			return BadRequest("Delete department operation failed");
			
        }
    }
}
