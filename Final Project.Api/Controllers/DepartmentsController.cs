using FinalProject.Core;
using FinalProject.Core.Dtos.DepartmentDots;
using FinalProject.Core.Dtos.DepartmentDtos;
using FinalProject.Core.Models;
using FinalProject.EF.Translation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace FinalProject.Api.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
		private readonly TranslationService _translationService;

		public DepartmentController(IUnitOfWork unitOfWork, TranslationService translationService)
        {
            _unitOfWork = unitOfWork;
			this._translationService = translationService;
		}

		// POST: api/Department/Create_Department
		[Authorize(Roles = "Admin")]
		[HttpPost("/Create_Department/{lang}")]
        public async Task<ActionResult<bool>> Create(string lang,[FromBody] CreateDepartmentDto departmentDto)
        {
            if (lang== "eng")
            {
                var department = new Department()
                {
                    Name = departmentDto.Name,
                    ArabicName = await _translationService.TranslateLongTextAsync(departmentDto.Name, "en", "ar"),
                    Description = departmentDto.Description,
                    ArabicDescription = await _translationService.TranslateLongTextAsync(departmentDto.Description, "en", "ar"),
                    Head_Of_Department = await _unitOfWork.Employees.GetByIdAsync(e => e.EmployeeId == departmentDto.HeadOfDepartmentId),
                    Head_Of_DepartmentId = departmentDto.HeadOfDepartmentId,

                };

                await _unitOfWork.Departments.AddAsync(department);

                int res = await _unitOfWork.CompleteAsync();
                if (res > 0)
                    return Ok(true);
                return BadRequest("Department Create operation failed");
            }
            else
            {
				var department = new Department()
				{
					ArabicName = departmentDto.Name,
					Name = await _translationService.TranslateLongTextAsync(departmentDto.Name, "ar", "en"),
					ArabicDescription = departmentDto.Description,
					Description = await _translationService.TranslateLongTextAsync(departmentDto.Description, "ar", "en"),
					Head_Of_Department = await _unitOfWork.Employees.GetByIdAsync(e => e.EmployeeId == departmentDto.HeadOfDepartmentId),
					Head_Of_DepartmentId = departmentDto.HeadOfDepartmentId,

				};

				await _unitOfWork.Departments.AddAsync(department);

				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
					return Ok(true);
				return BadRequest("لم يتم عمل القسم");
			}
		}

        // GET: api/Department/Get_Department_By_Id/{id}
        [HttpGet("/Get_Department_By_Id/{id}/{lang}")]
        public async Task<ActionResult<DepartmentDto>> Get(int id,string lang)
        {
            Department department = await _unitOfWork.Departments.GetByIdAsync(d => d.DepartmentId == id, new[] { "Head_Of_Department" });
            if (lang=="eng")
            {
            if (department == null) 
                return NotFound("Department not found");

                var mapped = new DepartmentDto
                {
                   
                    Description = department.Description,
                    Name = department.Name,
                    EmpImage = department.Head_Of_Department?.Image,
                    EmpJob_Title = department.Head_Of_Department?.Job_Title,
                    EmployeeId = department.Head_Of_Department?.EmployeeId,
                    EmpName = department.Head_Of_Department?.Name,
                    EmpResume = department.Head_Of_Department?.Resume,
                    
                };
                return Ok(mapped);
            }
            else
            {
				if (department == null)
					return NotFound("لا يوجد قسم");
				var mapped = new DepartmentDto
				{
                    
					Description = department.ArabicDescription,
					Name = department.ArabicName,
					EmpImage = department.Head_Of_Department?.Image,
					EmpJob_Title = department.Head_Of_Department?.ArabicJob_Title,
					EmployeeId = department.Head_Of_Department?.EmployeeId,
					EmpName = department.Head_Of_Department?.ArabicName,
					EmpResume = department.Head_Of_Department?.Resume
				};
				return Ok(mapped);
			}
        }
     
        // GET: api/Department/Get_All_Departments
        [HttpGet("/Get_All_Departments/{lang}")]
        public async Task<ActionResult<IEnumerable<DepartmentsToReturnDto>>> GetAll(string lang)
        {
            IEnumerable<Department> departments = await _unitOfWork.Departments.GetAllAsync(null);
            if (lang=="eng")
            {
            if (departments == null) return NotFound("There is no department created yet");
                
                return Ok(departments.Select(x=> new DepartmentsToReturnDto
				{
                    DepartmentId=x.DepartmentId,
                    Name=x.Name,
                }));
            }
            else
            {
            if (departments == null) return NotFound("لا يوجد اقسام");

				return Ok(departments.Select(x => new DepartmentsToReturnDto
				{
					DepartmentId = x.DepartmentId,
					Name = x.ArabicName,
				}));
			}
        }

        [HttpGet("Get_All_Departments_Details/{lang}")]
        public async Task<ActionResult<IEnumerable<DepartmentHeadDto>>> GetAllDetails(string lang)
        {
            IEnumerable<Department> departments = await _unitOfWork.Departments.GetAllAsync(null, new[] { "Head_Of_Department" });

            if (lang == "eng")
            {
                if (departments == null) return NotFound("There is no department created yet");

                return Ok(departments.Select(x => new DepartmentHeadDto
                {
                    Id = x.DepartmentId,
                    Name = x.Name,
                    Description = x.Description,
                    EmployeeId = x.Head_Of_Department.EmployeeId,
                    EmpName = x.Head_Of_Department.Name
                }));
            }
            else
            {
                if (departments == null) return NotFound("لا يوجد اقسام");

                return Ok(departments.Select(x => new DepartmentHeadDto
                {
                    Id = x.DepartmentId,
                    Name = x.ArabicName,
                    Description = x.ArabicDescription,
                    EmployeeId = x.Head_Of_Department.EmployeeId,
                    EmpName = x.Head_Of_Department.ArabicName
                }));

            }
        }
		// PUT: api/Department/Update_Department
		[Authorize(Roles = "Admin")]
		[HttpPut("/Update_Department/{id}/{lang}")]
        public async Task<ActionResult<bool>> Update(int id,string lang,[FromBody] CreateDepartmentDto departmentDto)
        {
            if (lang== "eng")
            {
                var department = await _unitOfWork.Departments.GetByIdAsync(d => d.DepartmentId == id);

                if (department == null)
                    return NotFound("Department not found");


                department.Name = departmentDto.Name;
                department.ArabicName = await _translationService.TranslateLongTextAsync(departmentDto.Name, "en", "ar");
                department.Description = departmentDto.Description;
                department.ArabicDescription = await _translationService.TranslateLongTextAsync(departmentDto.Description, "en", "ar");
                department.Head_Of_Department = await _unitOfWork.Employees.GetByIdAsync(e => e.EmployeeId == departmentDto.HeadOfDepartmentId);


                _unitOfWork.Departments.Update(department);

                int res = await _unitOfWork.CompleteAsync();
                if (res > 0)
                    return Ok(true);
                return BadRequest("Department Update operation failed");
            }
            else
            {
				var department = await _unitOfWork.Departments.GetByIdAsync(d => d.DepartmentId == id);

				if (department == null)
					return NotFound("القسم غير موجود");


				department.ArabicName = departmentDto.Name;
				department.Name = await _translationService.TranslateLongTextAsync(departmentDto.Name, "ar", "en");
				department.ArabicDescription = departmentDto.Description;
				department.Description = await _translationService.TranslateLongTextAsync(departmentDto.Description, "ar", "en");
				department.Head_Of_Department = await _unitOfWork.Employees.GetByIdAsync(e => e.EmployeeId == departmentDto.HeadOfDepartmentId);


				_unitOfWork.Departments.Update(department);

				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
					return Ok(true);
				return BadRequest("لم يتم تعديل القسم");
			}
		}
		[Authorize(Roles = "Admin")]
		[HttpPut("/Add_Emloyee_To_Departmet/{departmentId}/{employeeId}")]
        public async Task<ActionResult<bool?>> AddEmployeeToDepartment(int departmentId,int employeeId)
        {

            var employee = await _unitOfWork.Employees.GetByIdAsync(e => e.EmployeeId == employeeId);
            if (employee == null) return NotFound("Employee not found");
            employee.DepartmentId = departmentId;
            var department = await _unitOfWork.Departments.GetByIdAsync(d=> d.DepartmentId == departmentId);
            if (department == null) return NotFound("Department not found");

            department.Employees?.Add(employee);
			_unitOfWork.Departments.Update(department);

			int res = await _unitOfWork.CompleteAsync();
			if (res > 0)
				return Ok(true);
			return BadRequest("Employee added  failed");
			
        }

		[Authorize(Roles = "Admin")]
		[HttpPut("/Remove_Emloyee_From_Department/{departmentId}/{employeeId}")]
        public async Task<ActionResult<bool>> RemoveEmployeeFromDepartment(int departmentId, int employeeId)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(e => e.EmployeeId == employeeId);
            if (employee == null) return NotFound("Employee not found");
            employee.DepartmentId = null;
            var department = await _unitOfWork.Departments.GetByIdAsync(d => d.DepartmentId == departmentId);
            if (department == null) return NotFound("Department not found");

            department.Employees?.Remove(employee);
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
            Department department =await _unitOfWork.Departments.GetByIdAsync(d => d.DepartmentId == id);
            if (department == null) return NotFound("Department Not Found");

            _unitOfWork.Departments.Delete(department);
            
			int res = await _unitOfWork.CompleteAsync();
			if (res > 0)
				return Ok(department);
			return BadRequest("Delete department operation failed");
			
        }
    }
}
