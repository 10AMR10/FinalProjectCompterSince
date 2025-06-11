using DiabetesApp.API.Dtos;
using FinalProject.Api.Helpers;
using FinalProject.Core;
using FinalProject.Core.Dtos.DepartmentDtos;
using FinalProject.Core.Dtos.EmployeeDots;
using FinalProject.Core.Dtos.EmployeeDtos;
using FinalProject.Core.Models;
using FinalProject.Core.Models.identity;
using FinalProject.EF.Token;
using FinalProject.EF.Translation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.Errors;


namespace FinalProject.Api.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IConfiguration _configuration;
		private readonly TranslationService _translationService;
		private readonly ITokenService _tokenService;

		public EmployeeController(IUnitOfWork unitOfWork,UserManager<ApplicationUser> userManager,IConfiguration configuration, TranslationService translationService ,ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
			this._userManager = userManager;
			this._configuration = configuration;
			this._translationService = translationService;
			this._tokenService = tokenService;
		}

		// POST: api/Employee/Create_Employee
		[Authorize(Roles = "Admin")]
		[HttpPost("Create_Employee/{lang}")]
        public async Task<ActionResult<bool>> Create(string lang, EmployeeCreateDto employeeDto)
        {
			if (lang=="eng")
			{
				var employee = new Employee()
				{
					Name = employeeDto.Name,
					ArabicName = await _translationService.TranslateLongTextAsync(employeeDto.Name, "en", "ar"),


					Job_Title = employeeDto.Job_Title,
					ArabicJob_Title = await _translationService.TranslateLongTextAsync(employeeDto.Job_Title, "en", "ar"),
					
				};

				var resume = FileMangment.UploadFile(employeeDto.Resume, _configuration);
				if (resume is null)
					return BadRequest("Extention Or Size Not Valid For Cv");
				employee.Resume = resume;
				var imag = FileMangment.UploadFile(employeeDto.Image, _configuration);
				if (imag == null)
					return BadRequest("Extention Or Size Not Valid For Image");
				employee.Image = imag;


				

				if (await _userManager.FindByEmailAsync(employeeDto.email) is not null)
					return BadRequest(new ApiResponse(400, "Dublicated Email"));
				employee.Email = employeeDto.email;

				
				
				await _unitOfWork.Employees.AddAsync(employee);
				int res = await _unitOfWork.CompleteAsync();
				if (res == 0)
					return BadRequest("Employee creation failed");

				var user = new ApplicationUser
				{
					Email = employeeDto.email,
					EmployeId=employee.EmployeeId,
					UserName = employee.Name.Replace(" ", ""),
				};



				var resu = await _userManager.CreateAsync(user, employeeDto.password);

				if (!resu.Succeeded)
				{
					// Collect detailed error messages
					var errorDetails = resu.Errors
						.Select(e => $"Code: {e.Code}, Description: {e.Description}")
						.ToList();

					// Return a detailed BadRequest response
					return BadRequest(new ApiResponse(400, string.Join(" | ", errorDetails)));
				}
				await _userManager.AddToRoleAsync(user, "Doctor");

				
				return Ok(true);
			}
			else
			{
				var employee = new Employee()
				{
					ArabicName = employeeDto.Name,
					Name = await _translationService.TranslateLongTextAsync(employeeDto.Name, "ar", "en"),
					

					ArabicJob_Title = employeeDto.Job_Title,
					Job_Title = await _translationService.TranslateLongTextAsync(employeeDto.Job_Title, "ar", "en"),
					//Resume =  _unitOfWork.Employees.UploadEmployeeCV(employeeDto.Resume , null).ToString(),
					//DepartmentId = employeeDto.DepartmentId
					//Department =await _unitOfWork.Departments.GetByIdAsync(d => d.DepartmentId == employeeDto.DepartmentId)
				};
				var resume = FileMangment.UploadFile(employeeDto.Resume, _configuration);
				if (resume is null)
					return BadRequest("الحجم او الاضافه غير صحيح");
				employee.Resume = resume;
				var imag = FileMangment.UploadFile(employeeDto.Image, _configuration);
				if (imag == null)
					return BadRequest("الحجم او الاضافه غير صحيح");
				employee.Image = imag;

			

				if (await _userManager.FindByEmailAsync(employeeDto.email) is not null)
					return BadRequest(new ApiResponse(400, "الايميل متكرر"));
				employee.Email = employeeDto.email;
				
				
				await _unitOfWork.Employees.AddAsync(employee);
				int res = await _unitOfWork.CompleteAsync();
				if (res == 0)
					return BadRequest("لم يتم عمل موظف");

				var user = new ApplicationUser
				{
					Email = employeeDto.email,
					EmployeId = employee.EmployeeId,
					UserName = employee.Name.Replace(" ", ""),
				};



				var resu = await _userManager.CreateAsync(user, employeeDto.password);

				if (!resu.Succeeded)
				{
					// Collect detailed error messages
					var errorDetails = resu.Errors
						.Select(e => $"Code: {e.Code}, Description: {e.Description}")
						.ToList();

					// Return a detailed BadRequest response
					return BadRequest(new ApiResponse(400, string.Join(" | ", errorDetails)));
				}
				await _userManager.AddToRoleAsync(user, "Doctor");


				return Ok(true);




			}
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
		public async Task<ActionResult<EmployeeDto>> Get(int id, string lang)
		{
			Employee employee = await _unitOfWork.Employees.GetByIdAsync(e => e.EmployeeId == id, new[] { "Department" });
			if (lang == "eng")
			{
			if (employee == null)
				return NotFound("Epmloyee not found");
				var mapped = new EmployeeDto()
				{
					Name = employee.Name,
					EmployeeId = employee.EmployeeId,
					Job_Title = employee.Job_Title,
					DepartmentName = employee.Department?.Name is not null ? employee.Department.Name : "Non",
					Resume = employee.Resume,
					Image= employee.Image,
					DepartmentId=employee.DepartmentId ,
					Email= employee.Email,

				};
				return Ok(mapped);
			}
			else
			{
				if (employee == null)
					return NotFound("لا يوجد موظفين");
				var mapped = new EmployeeDto()
				{
					Name = employee.ArabicName,
					EmployeeId = employee.EmployeeId,
					Job_Title = employee.ArabicJob_Title,
					DepartmentName = employee.Department?.ArabicName is not null ? employee.Department.ArabicName : "ليس فى قسم",
					Resume = employee.Resume,
					Image = employee.Image,
					DepartmentId = employee.DepartmentId,
					Email = employee.Email,
				};
				return Ok(mapped);
			}


		}
		[HttpGet("/Get_All_Employees/{lang}")]
		public async Task<ActionResult<IReadOnlyList<EmployeeDto>>> GetEmployees( string lang)
		{
			var employees = await _unitOfWork.Employees.GetAllAsync(null, new[] { "Department" });
			if (lang == "eng")
			{
				if (employees == null)
					return NotFound("Epmloyee not found");
				var mapped = employees.Select(x=> new EmployeeDto()
				{
					Name = x.Name,
					EmployeeId = x.EmployeeId,
					Job_Title = x.Job_Title,
					DepartmentName = x.Department?.Name is not null ? x.Department.Name : "Non",
					Resume = x.Resume,
					Image=x.Image,
					DepartmentId = x.DepartmentId,
					Email= x.Email,

				});
				return Ok(mapped);
			}
			else
			{
				if (employees == null)
					return NotFound("لا يوجد موظفين");
				var mapped = employees.Select(x => new EmployeeDto()
				{
					Name = x.ArabicName,
					EmployeeId = x.EmployeeId,
					Job_Title = x.ArabicJob_Title,
					DepartmentName = x.Department?.ArabicName is not null ? x.Department.ArabicName : "Non",
					Resume = x.Resume,
					Image = x.Image,
					DepartmentId = x.DepartmentId,
					Email = x.Email,

				});
				return Ok(mapped);
			}


		}

		// GET: api/Employee/Get_All_Employees

		[HttpGet("/Get_All_Employees_In_Department/{departmentId}/{lang}")]
        public async Task<ActionResult<IReadOnlyList<EmployeeToReturnDto>>> GetAllEmployees(int departmentId,string lang)
        {

            var employees =await _unitOfWork.Employees.GetAllAsync(x=> x.Department.DepartmentId==departmentId,new[] { "Department"});
			if (lang=="eng")
			{
            if (employees == null) return NotFound("There is no employee added yet.");

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
				if (employees == null)
					return NotFound("لا يوجد موظفين");
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
			if (lang == "eng")
			{
			if (employee == null) return NotFound("There is no employee added yet.");

				var mapped =new EmployeeToReturnDto
				{
					EmployeeId = employee.EmployeeId,
					DepartmentName = employee.Department?.Name,
					Job_Title = employee.Job_Title,
					Name = employee.Name,
					Resume = employee.Resume
				};
				return Ok(mapped);
			}
			else
			{
				if (employee == null)
					return NotFound("لا يوجد موظفين");
				var mapped = new EmployeeToReturnDto
				{
					EmployeeId = employee.EmployeeId,
					DepartmentName = employee.Department?.ArabicName,
					Job_Title = employee.ArabicJob_Title,
					Name = employee.ArabicName,
					Resume = employee.Resume,

				};
				return Ok(mapped);
			}

		}

		// PUT: api/Employee/Update_Employee
		[Authorize(Roles = "Admin,Doctor")]
		[HttpPut("/Update_Employee/{id}/{lang}")]
        public async Task<ActionResult<bool>> Update(int id,string lang,EmployeeUpdateDto employeeDto)
        {
			if (lang=="eng")
			{
				var employee = await _unitOfWork.Employees.GetByIdAsync(e => e.EmployeeId == id, new[] { "Department" });
				if (employee == null) return NotFound("Employee not found");


				employee.Name = employeeDto.Name;
				employee.ArabicName = await _translationService.TranslateLongTextAsync(employeeDto.Name, "en", "ar");
				
				employee.Job_Title = employeeDto.Job_Title;
				employee.ArabicJob_Title = await _translationService.TranslateLongTextAsync(employeeDto.Job_Title, "en", "ar");
				var resume = FileMangment.UploadFile(employeeDto.Resume, _configuration);
				if (resume is null)
					return BadRequest("Extention Or Size Not Valid For Cv");
				employee.Resume = resume;
				var imag = FileMangment.UploadFile(employeeDto.Image, _configuration);
				if (imag == null)
					return BadRequest("Extention Or Size Not Valid For Image");
				employee.Image = imag;
				//DepartmentId = employeeDto.DepartmentId,
				if (employeeDto.DepartmentId is not null)
				{
					employee.Department = await _unitOfWork.Departments.GetByIdAsync(d => d.DepartmentId == employeeDto.DepartmentId);

				}
				var user = _userManager.Users.Where(x => x.EmployeId == id).FirstOrDefault();
				if (user?.Email != employeeDto.email)
				{
					if (await _userManager.FindByEmailAsync(employeeDto.email) is not null)
						return BadRequest(new ApiResponse(400, "Dublicated Email"));

					if (user != null)
					{
						user.Email = employeeDto.email;
						var re = await _userManager.UpdateAsync(user);
						if (!re.Succeeded)
						{
							// Collect detailed error messages
							var errorDetails = re.Errors
								.Select(e => $"Code: {e.Code}, Description: {e.Description}")
								.ToList();

							// Return a detailed BadRequest response
							return BadRequest(new ApiResponse(400, string.Join(" | ", errorDetails)));
						}
					}
				}
				_unitOfWork.Employees.Update(employee);

				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
					return Ok(true);
				return BadRequest("Employee update operation failed");
			}
			else
			{
				var employee = await _unitOfWork.Employees.GetByIdAsync(e => e.EmployeeId == id, new[] { "Department" });
				if (employee == null) return NotFound("الموظف غير موجود");


				employee.ArabicName = employeeDto.Name;
				employee.Name = await _translationService.TranslateLongTextAsync(employeeDto.Name, "ar", "en");
				employee.Email = employeeDto.email;
				employee.ArabicJob_Title = employeeDto.Job_Title;
				employee.Job_Title = await _translationService.TranslateLongTextAsync(employeeDto.Job_Title, "ar", "en");
				var resume = FileMangment.UploadFile(employeeDto.Resume, _configuration);
				if (resume is null)
					return BadRequest("الحجم او الاضافه غير صحيح");
				employee.Resume = resume;
				var imag = FileMangment.UploadFile(employeeDto.Image, _configuration);
				if (imag == null)
					return BadRequest("الحجم او الاضافه غير صحيح");
				employee.Image = imag;
				//DepartmentId = employeeDto.DepartmentId,
				if (employeeDto.DepartmentId is not null)
				{
					employee.Department = await _unitOfWork.Departments.GetByIdAsync(d => d.DepartmentId == employeeDto.DepartmentId);

				}
				var user = _userManager.Users.Where(x => x.EmployeId == id).FirstOrDefault();
				if (user?.Email!=employeeDto.email)
				{
					if (await _userManager.FindByEmailAsync(employeeDto.email) is not null)
						return BadRequest(new ApiResponse(400, "Dublicated Email"));
					
					if (user != null)
					{
						user.Email = employeeDto.email;
						var re = await _userManager.UpdateAsync(user);
						if (!re.Succeeded)
						{
							// Collect detailed error messages
							var errorDetails = re.Errors
								.Select(e => $"Code: {e.Code}, Description: {e.Description}")
								.ToList();

							// Return a detailed BadRequest response
							return BadRequest(new ApiResponse(400, string.Join(" | ", errorDetails)));
						}
					} 
				}
				_unitOfWork.Employees.Update(employee);

				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
					return Ok(true);
				return BadRequest("لم يتم تعديل الموظف");
				
				
			}
		}

		// DELETE: api/Employee/Delete_Employee/{id}
		[Authorize(Roles = "Admin")]
		[HttpDelete("/Delete_Employee/{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var employee =await _unitOfWork.Employees.GetByIdAsync(e => e.EmployeeId == id);

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
