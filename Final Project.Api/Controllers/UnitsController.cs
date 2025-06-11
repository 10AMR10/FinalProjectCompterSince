using FinalProject.Api.Helpers;
using FinalProject.Core;
using FinalProject.Core.Dtos.UnitDots;
using FinalProject.Core.Models;
using FinalProject.EF.Translation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace FinalProject.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UnitController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IConfiguration _configuration;
		private readonly TranslationService _translationService;

		public UnitController(IUnitOfWork unitOfWork,IConfiguration configuration, TranslationService translationService)
		{
			_unitOfWork = unitOfWork;
			this._configuration = configuration;
			this._translationService = translationService;
		}

		// POST: api/Unit
		[Authorize(Roles = "Admin")]
		[HttpPost("{lang}")]
		public async Task<ActionResult<bool>> Create(string lang, UnitCreateDto unitDto)
		{
			if (lang=="eng")
			{
				if (unitDto == null)
					return BadRequest("Invalid unit data.");



				var unit = new Unit()
				{
					Name = unitDto.Name,
					ArabicName = await _translationService.TranslateLongTextAsync(unitDto.Name, "en", "ar"),
					ArabicDescription = await _translationService.TranslateLongTextAsync(unitDto.Description, "en", "ar"),
					Description = unitDto.Description,

				};

				await _unitOfWork.Units.AddAsync(unit);

				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
					return Ok(true);
				return BadRequest("Failed to create unit.");
			}
			else
			{
				if (unitDto == null)
					return BadRequest("الوحده غير موجوده");



				var unit = new Unit()
				{
					ArabicName = unitDto.Name,
					Name = await _translationService.TranslateLongTextAsync(unitDto.Name, "ar", "en"),
					Description = await _translationService.TranslateLongTextAsync(unitDto.Description, "ar", "en"),
					ArabicDescription = unitDto.Description,

				};

				await _unitOfWork.Units.AddAsync(unit);

				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
					return Ok(true);
				return BadRequest("لم يتم عمل الوحده");
			}
		}

		// GET: api/Unit/{id}
		[HttpGet("{id}/{lang}")]
		public async Task<ActionResult<UnitDto>> GetById(int id, string lang)
		{
			var unit = await _unitOfWork.Units.GetByIdAsync(u => u.UnitId == id);
			if (unit == null)
				return NotFound("Unit not Found");
			if (lang == "eng")
			{
				var mapped = new UnitDto
				{
					Id = id,
					Description = unit.Description,
					Name = unit.Name,
				};
				return Ok(mapped);
			}
			else
			{
				var mapped = new UnitDto
				{
					Id = id,
					Description = unit.ArabicDescription,
					Name = unit.ArabicName,
				};
				return Ok(mapped);
			}
		}

		// GET: api/Unit
		[HttpGet("{lang}")]
		public async Task<ActionResult<IEnumerable<UnitDto>>> GetAll(string lang)
		{
			var units = await _unitOfWork.Units.GetAllAsync(null);
			if (units == null)
				return NotFound("No Units Are Found");
			if (lang == "eng")
			{
				var mapped = units.Select(x => new UnitDto
				{
					Id = x.UnitId,
					Description = x.Description,
					Name = x.Name,
				});
				return Ok(mapped);
			}
			else
			{
				var mapped = units.Select(x => new UnitDto
				{
					Id = x.UnitId,
					Description = x.ArabicDescription,
					Name = x.ArabicName,
				});
				return Ok(mapped);
			}
		}

		[HttpGet("Unit_All_Employees/{lang}")]
		public async Task<ActionResult<IEnumerable<UnitEmployeeToReturnDto>>> GetAllEmployees(string lang)
		{
			var employees = await _unitOfWork.UnitEmployees.GetAllAsync(null, new[] { "Unit" });
			if (lang == "eng")
			{
				return Ok(employees.Select(x=> new UnitEmployeeToReturnDto
				{
					Id=x.Id,
					Job_Title=x.Job_Title,
					Name=x.Name,
					Resume=x.Resume,
					UnitId=x.UnitId,
					UnitName=x.Unit.Name,

				}));
			}
			else
			{
				return Ok(employees.Select(x => new UnitEmployeeToReturnDto
				{
					Id = x.Id,
					Job_Title = x.ArabicJob_Title,
					Name = x.ArabicName,
					Resume = x.Resume,
					UnitId = x.UnitId,
					UnitName = x.Unit.ArabicName,

				}));
			}
		}


		[HttpGet("Unit_All_Courses/{lang}")]
		public async Task<ActionResult<IEnumerable<UnitCoursesToRetrunDto>>> GetAllCourses(string lang)
		{
			var courses = await _unitOfWork.UnitCourses.GetAllAsync(null, new[] { "unit" });
			if (lang == "eng")
			{
				return Ok(courses.Select(x => new UnitCoursesToRetrunDto
				{
					Id = x.Id,
					PdfDescription = x.PdfDescription,
					Title = x.Title,
					UnitId= x.UnitId,
					UnitName=x.unit.Name,
					
					
				}));
			}
			else
			{
				return Ok(courses.Select(x => new UnitCoursesToRetrunDto
				{
					Id = x.Id,
					PdfDescription = x.PdfDescription,
					Title = x.ArabicTitle,
					UnitId = x.UnitId,
					UnitName = x.unit.ArabicName,


				}));
			}
		}

		// PUT: api/Unit/
		[Authorize(Roles = "Admin")]
		[HttpPut("{id}/{lang}")]
		public async Task<ActionResult<Unit?>> Update(int id,string lang, [FromBody] UnitCreateDto unitDto)
		{

			if (lang=="eng")
			{
				var unit = await _unitOfWork.Units.GetByIdAsync(u => u.UnitId == id);
				if (unit == null)
					return NotFound("Unit not found");

				unit.Name = unitDto.Name;
				unit.ArabicName = await _translationService.TranslateLongTextAsync(unitDto.Name, "en", "ar");
				unit.ArabicDescription = await _translationService.TranslateLongTextAsync(unitDto.Description, "en", "ar");
				unit.Description = unitDto.Description;

				_unitOfWork.Units.Update(unit);

				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
					return Ok(unit);
				return BadRequest("Unit Update operation failed");
			}
			else
			{
				var unit = await _unitOfWork.Units.GetByIdAsync(u => u.UnitId == id);
				if (unit == null)
					return NotFound("الوحده غير موجوده");

				unit.ArabicName = unitDto.Name;
				unit.Name = await _translationService.TranslateLongTextAsync(unitDto.Name, "ar", "en");
				unit.Description = await _translationService.TranslateLongTextAsync(unitDto.Description, "ar", "en");
				unit.ArabicDescription = unitDto.Description;

				_unitOfWork.Units.Update(unit);

				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
					return Ok(unit);
				return BadRequest("لم يتم تعديل الوحده");
			}

		}
		
		// DELETE: api/Unit/{id}
		[Authorize(Roles = "Admin")]
		[HttpDelete("{id}")]
		public async Task<ActionResult<Unit?>> Delete(int id)
		{
			var unit = await _unitOfWork.Units.GetByIdAsync(u => u.UnitId == id);
			if (unit == null)
				return NotFound("Unit not found.");

			_unitOfWork.Units.Delete(unit);

			int res = await _unitOfWork.CompleteAsync();
			if (res > 0)
				return Ok(true);
			return BadRequest("Unit delete operation failed");
		}
		[Authorize(Roles = "Admin")]
		[HttpPost("/Add_Emloyee_To_Unit/{unitId}/{lang}")]
		public async Task<ActionResult<bool>> AddEmployeeToUnit(int unitId,string lang, AddEmployeeToUnitDto input)
		{
			if (lang=="eng")
			{
				var unit = await _unitOfWork.Units.GetByIdAsync(x => x.UnitId == unitId);
				if (unit == null) return BadRequest("No Unit Created ");
				var employee = new UnitEmployees
				{
					UnitId = unitId,
					ArabicName = await _translationService.TranslateLongTextAsync(input.Name, "en", "ar"),
					Name = input.Name,
					ArabicJob_Title = await _translationService.TranslateLongTextAsync(input.Job_Title, "en", "ar"),
					Job_Title = input.Job_Title,
					Unit = unit,
				};
				unit.UnitEmployees?.Add(employee);
				employee.Resume = FileMangment.UploadFile(input.Resume, _configuration);
				if (employee.Resume == null)
					return BadRequest("Extention Or Size Not Valid For Cv");
				await _unitOfWork.UnitEmployees.AddAsync(employee);
				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
					return Ok(true);
				return BadRequest("Failed to create Employee.");
			}
			else
			{
				var unit = await _unitOfWork.Units.GetByIdAsync(x => x.UnitId == unitId);
				if (unit == null) return BadRequest("لم يتم انشاء وحده");
				var employee = new UnitEmployees
				{
					UnitId = unitId,
					Name = await _translationService.TranslateLongTextAsync(input.Name, "ar", "en"),
					ArabicName = input.Name,
					Job_Title = await _translationService.TranslateLongTextAsync(input.Job_Title, "ar", "en"),
					ArabicJob_Title = input.Job_Title,
					Unit = unit,
				};
				unit.UnitEmployees?.Add(employee);
				employee.Resume = FileMangment.UploadFile(input.Resume, _configuration);
				if (employee.Resume == null)
					return BadRequest("الحجم او الاضافه غير صحيح");
				await _unitOfWork.UnitEmployees.AddAsync(employee);
				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
					return Ok(true);
				return BadRequest("لم يتم عمل موظف للوحده");
			}
		}
		[Authorize(Roles = "Admin")]
		[HttpPut("/Remove_Emloyee_From_Unit/{empId}")]
		public async Task<ActionResult<bool>> RemoveEmployeeFromUnit(int empId)
		{
			var employee = await _unitOfWork.UnitEmployees.GetByIdAsync(u => u.Id== empId);
			if (employee == null)
				return NotFound("course not found.");

			_unitOfWork.UnitEmployees.Delete(employee);

			int res = await _unitOfWork.CompleteAsync();
			if (res > 0)
				return Ok(true);
			return BadRequest("Unit delete operation failed");

		}
		[HttpGet("Get_Employee_By_UnitId{unitId}/{lang}")]
		public async Task<ActionResult<IReadOnlyList< UnitsEmployeeDto>>> GetEmployeeInUnit(int unitId, string lang)
		{
			var unit = await _unitOfWork.Units.GetByIdAsync(x => x.UnitId == unitId, new[] { "UnitEmployees" });
			if (unit == null)
				return BadRequest("Sorry No Unit. ");
			var employees = unit.UnitEmployees;
			if (lang == "eng")
			{
				var mapped = employees.Select(x => new UnitsEmployeeDto
				{
					Id = x.Id,
					Job_Title = x.Job_Title,
					Name = x.Name,
					Resume = x.Resume,
					UnitName = unit.Name,
				});
				return Ok(mapped);
			}
			else
			{
				var mapped = employees.Select(x => new UnitsEmployeeDto
				{
					Id = x.Id,
					Job_Title = x.ArabicJob_Title,
					Name = x.ArabicName,
					Resume = x.Resume,
					UnitName = unit.ArabicName,
				});
				return Ok(mapped);
			}
		}
		[Authorize(Roles = "Admin")]
		[HttpPost("/Add_Course_To_Unit/{unitId}/{lang}")]
		public async Task<ActionResult<bool>> AddCourseToUnit(int unitId,string lang, AddCourseToUnitDto input)
		{
			if (lang=="eng")
			{
				var unit = await _unitOfWork.Units.GetByIdAsync(x => x.UnitId == unitId);
				if (unit == null) return BadRequest("No Unit Created ");
				var course = new UnitCourses
				{
					UnitId = unitId,
					ArabicTitle = await _translationService.TranslateLongTextAsync(input.Title, "en", "ar"),
					Title = input.Title,
					unit = unit,
				};
				unit.unitCourses.Add(course);
				course.PdfDescription = FileMangment.UploadFile(input.PdfDescription, _configuration);
				if (course.PdfDescription == null)
					return BadRequest("Extention Or Size Not Valid For Cv");
				await _unitOfWork.UnitCourses.AddAsync(course);
				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
					return Ok(true);
				return BadRequest("Failed to create Employee.");
			}
			else
			{
				var unit = await _unitOfWork.Units.GetByIdAsync(x => x.UnitId == unitId);
				if (unit == null) return BadRequest("لم يتم انشاء وحده ");
				var course = new UnitCourses
				{
					UnitId = unitId,
					Title = await _translationService.TranslateLongTextAsync(input.Title, "ar", "en"),
					ArabicTitle = input.Title,
					unit = unit,
				};
				unit.unitCourses.Add(course);
				course.PdfDescription = FileMangment.UploadFile(input.PdfDescription, _configuration);
				if (course.PdfDescription == null)
					return BadRequest("الحجم او الاضافه غير صحيح");
				await _unitOfWork.UnitCourses.AddAsync(course);
				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
					return Ok(true);
				return BadRequest("لم يتم اضافه كورس للوحده");
			}
		}
		[Authorize(Roles = "Admin")]
		[HttpPut("/Remove_Course_From_Unit/{courseId}")]
		public async Task<ActionResult<bool>> RemoveCourseFromUnit(int courseId)
		{
			var course = await _unitOfWork.UnitCourses.GetByIdAsync(u => u.Id == courseId);
			if (course == null)
				return NotFound("course not found.");

			_unitOfWork.UnitCourses.Delete(course);

			int res = await _unitOfWork.CompleteAsync();
			if (res > 0)
				return Ok(true);
			return BadRequest("course delete operation failed");

		}
		[HttpGet("Get_Course_By_UnitId{unitId}/{lang}")]
		public async Task<ActionResult<IReadOnlyList<UnitCourseDto>>> GetCourseInUnit(int unitId, string lang)
		{
			var unit = await _unitOfWork.Units.GetByIdAsync(x => x.UnitId == unitId, new[] { "unitCourses" });
			if (unit == null)
				return BadRequest("Sorry No Unit. ");
			var course = unit.unitCourses;
			if (lang == "eng")
			{
				var mapped = course.Select(x => new UnitCourseDto
				{
					Id = x.Id,
					PdfDescription = x.PdfDescription,
					Title = x.Title,
				});
				return Ok(mapped);
			}
			else
			{
				var mapped = course.Select(x => new UnitCourseDto
				{
					Id = x.Id,
					PdfDescription = x.PdfDescription,
					Title = x.ArabicTitle,
				});
				return Ok(mapped);
			}
		}
	}
}
