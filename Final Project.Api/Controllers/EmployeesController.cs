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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FinalProject.Core.Models.identity;
using System.Security.Claims;
using FinalProject.Core.Dtos.EmployeeDtos;
using FinalProject.Api.Helpers;
using Microsoft.AspNetCore.Mvc.Razor;


namespace FinalProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<ApplicationUser> _userManager;

		public EmployeeController(IUnitOfWork unitOfWork,UserManager<ApplicationUser> userManager )
        {
            _unitOfWork = unitOfWork;
			this._userManager = userManager;
		}

		// POST: api/Employee/Create_Employee
		[Authorize(Roles = "Admin")]
		[HttpPost("/Create_Employee")]
        public async Task<ActionResult<Employee>> Create(EmployeeCreateDto employeeDto)
        {
            var employee = new Employee()
            {
                Name = employeeDto.Name,
                ArabicName= employeeDto.ArabicName,
                Email = employeeDto.Email,
                
                Job_Title = employeeDto.Job_Title,
                ArabicJob_Title=employeeDto.ArabicJob_Title,
                //Resume =  _unitOfWork.Employees.UploadEmployeeCV(employeeDto.Resume , null).ToString(),
                //DepartmentId = employeeDto.DepartmentId
                //Department =await _unitOfWork.Departments.GetByIdAsync(d => d.DepartmentId == employeeDto.DepartmentId)
            };
			if (FileMangment.UploadFile(employeeDto.Resume) == null)
				return BadRequest("Extention Or Size Not Valid For Cv");
			employee.Resume = FileMangment.UploadFile(employeeDto.Resume);
			if (FileMangment.UploadFile(employeeDto.Image) == null)
				return BadRequest("Extention Or Size Not Valid For Image");
			employee.Image = FileMangment.UploadFile(employeeDto.Image);

			await _unitOfWork.Employees.AddAsync(employee);
           
            int res=await _unitOfWork.CompleteAsync();
            if(res>0)
                 return Ok(employee);
            return BadRequest("Employee creation failed");

        }
		//[HttpPost("/Upload_CV")]
		//      public async Task<ActionResult<bool>> UploadCV( int employeeId , IFormFile CVFile)
		//      {
		//          var employee = await _unitOfWork.Employees.GetByIdAsync(e => e.EmployeeId == employeeId);
		//          if (employee == null) return NotFound("Employee not found");

		//          //var uploadDirectory = _hostEnvironment.WebRootPath ??
		//          //    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" ,"uploads") ;

		//          //Directory.CreateDirectory(uploadDirectory);

		//          if( FileMangment.UploadFile( CVFile ) == null)
		//		return BadRequest("Extention Or Size Not Valid");
		//          employee.Resume = FileMangment.UploadFile(CVFile);
		//	int res = await _unitOfWork.CompleteAsync();
		//	if (res > 0)
		//		return Ok(true);

		//	return BadRequest("CV Upload failed");

		//}

		// GET: api/Employee/Get_Employee_By_Id/{id}
		#region get user 
		//[Authorize(Roles = "Admin, Doctor")]
		//[HttpGet("/Get_Employee_By_Id_Users/{id}/{lang}")]
		//public async Task<ActionResult<EmployeeToReturnDto>> GetUsers(int id, string lang)
		//{

		//	Employee employee = await _unitOfWork.Employees.GetByIdAsync(e => e.EmployeeId == id, new[] { "Department" });
		//	var email = User.FindFirstValue(ClaimTypes.Email);
		//	var user = await _userManager.FindByEmailAsync(email);
		//	var role = await _userManager.GetRolesAsync(user);

		//	if (employee == null)
		//		return NotFound("Epmloyee not found");
		//	if (lang == "eng")
		//	{
		//		var mapped = new EmployeeToReturnDto()
		//		{
		//			Name = employee.Name,
		//			EmployeeId = employee.EmployeeId,
		//			Job_Title = employee.Job_Title,
		//			DepartmentName = employee.Department.Name is not null ? employee.Department.Name : "Non",
		//			Resume = employee.Resume,

		//		};
		//		return Ok(mapped);
		//	}
		//	else
		//	{
		//		var mapped = new EmployeeToReturnDto()
		//		{
		//			Name = employee.ArabicName,
		//			EmployeeId = employee.EmployeeId,
		//			Job_Title = employee.ArabicJob_Title,
		//			DepartmentName = employee.Department.ArabicName is not null ? employee.Department.ArabicName : "Non",
		//			Resume = employee.Resume,

		//		};
		//		return Ok(mapped);
		//	}


		//} 
		#endregion
		//get for public
		[HttpGet("/Get_Employee_By_Id/{id}/{lang}")]
		public async Task<ActionResult<EmployeeToReturnDto>> Get(int id, string lang)
		{
			Employee employee = await _unitOfWork.Employees.GetByIdAsync(e => e.EmployeeId == id, new[] { "Department" });
			if (employee == null)
				return NotFound("Epmloyee not found");
			if (lang == "eng")
			{
				var mapped = new EmployeeToReturnDto()
				{
					Name = employee.Name,
					EmployeeId = employee.EmployeeId,
					Job_Title = employee.Job_Title,
					DepartmentName = employee.Department.Name is not null ? employee.Department.Name : "Non",
					Resume = employee.Resume,

				};
				return Ok(mapped);
			}
			else
			{
				var mapped = new EmployeeToReturnDto()
				{
					Name = employee.ArabicName,
					EmployeeId = employee.EmployeeId,
					Job_Title = employee.ArabicJob_Title,
					DepartmentName = employee.Department.ArabicName is not null ? employee.Department.ArabicName : "Non",
					Resume = employee.Resume,

				};
				return Ok(mapped);
			}


		}
		// GET: api/Employee/Get_All_Employees

		[HttpGet("/Get_All_Employees/{departmentId}/{lang}")]
        public async Task<ActionResult<IReadOnlyList<EmployeeToReturnDto>>> GetAllEmployees(int departmentId,string lang)
        {

            IEnumerable<Employee> employees =await _unitOfWork.Employees.GetAllAsync(x=> x.Department.DepartmentId==departmentId,new[] { "Department"});
            if (employees == null) return NotFound("There is no employee added yet.");
			if (lang=="eng")
			{

				var mapped = employees.Select(x => new EmployeeToReturnDto
				{
					EmployeeId = x.EmployeeId,
					DepartmentName = x.Department.Name,
					Job_Title = x.Job_Title,
					Name = x.Name,
					Resume = x.Resume
				});
				return Ok(mapped);
			}
			else
			{
				var mapped = employees.Select(x => new EmployeeToReturnDto
				{
					EmployeeId = x.EmployeeId,
					DepartmentName = x.Department.ArabicName,
					Job_Title = x.ArabicJob_Title,
					Name = x.ArabicName,
					Resume = x.Resume,
					
				});
				return Ok(mapped);
			}
        }
		[Authorize(Roles = " Doctor")]
		[HttpGet("/Get_Doctor_Users/{lang}")]
		public async Task<ActionResult<IEnumerable<EmployeeToReturnDto>>> GetDoctorEmployeeDetails(string lang)
		{
			var email = User.FindFirstValue(ClaimTypes.Email);
			var user = await _userManager.FindByEmailAsync(email);

			var employee = await _unitOfWork.Employees.GetByIdAsync(x=>x.EmployeeId==user.EmployeId, new[] { "Department" });
			if (employee == null) return NotFound("There is no employee added yet.");
			if (lang == "eng")
			{

				var mapped =new EmployeeToReturnDto
				{
					EmployeeId = employee.EmployeeId,
					DepartmentName = employee.Department.Name,
					Job_Title = employee.Job_Title,
					Name = employee.Name,
					Resume = employee.Resume
				};
				return Ok(mapped);
			}
			else
			{
				var mapped = new EmployeeToReturnDto
				{
					EmployeeId = employee.EmployeeId,
					DepartmentName = employee.Department.ArabicName,
					Job_Title = employee.ArabicJob_Title,
					Name = employee.ArabicName,
					Resume = employee.Resume,

				};
				return Ok(mapped);
			}

		}

		// PUT: api/Employee/Update_Employee
		[Authorize(Roles = "Admin")]
		[HttpPut("/Update_Employee/{id}")]
        public async Task<ActionResult<bool>> Update(int id,[FromBody] EmployeeUpdateDto employeeDto)
        {
            Employee employee =await _unitOfWork.Employees.GetByIdAsync(e => e.EmployeeId == id, new[] { "Department" });
            if (employee == null) return NotFound("Employee not found");


			employee.Name = employeeDto.Name;
			employee.ArabicName = employeeDto.ArabicName;
			employee.Email = employeeDto.Email;
			employee.Job_Title = employeeDto.Job_Title;
			employee.ArabicJob_Title = employeeDto.ArabicJob_Title;
			employee.Resume = FileMangment.UploadFile(employeeDto.Resume);
			employee.Image = FileMangment.UploadFile(employeeDto.Image);
			//DepartmentId = employeeDto.DepartmentId,
			employee.Department = await _unitOfWork.Departments.GetByIdAsync(d => d.DepartmentId == employeeDto.DepartmentId);

			_unitOfWork.Employees.Update(employee);
           
			int res = await _unitOfWork.CompleteAsync();
			if (res > 0)
				return Ok(employee);
			return BadRequest("Employee update operation failed");
		}

		// DELETE: api/Employee/Delete_Employee/{id}
		[Authorize(Roles = "Admin")]
		[HttpDelete("/Delete_Employee/{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var employee =await _unitOfWork.Employees.GetByIdAsync(e => e.EmployeeId == id, new[] { "Department" });

            if (employee == null)
                return NotFound("Employee Not Found");

            _unitOfWork.Employees.Delete(employee);
            
			int res = await _unitOfWork.CompleteAsync();
			if (res > 0)
				return Ok(true);
			return BadRequest("Employee Delete operation failed");
		}
    }
}
