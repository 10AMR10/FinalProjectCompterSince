using FinalProject.Api.Helpers;
using FinalProject.Core;
using FinalProject.Core.Dtos.UnitDots;
using FinalProject.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace FinalProject.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UnitController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;

		public UnitController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		// POST: api/Unit
		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<ActionResult<bool>> Create(UnitCreateDto unitDto)
		{
			if (unitDto == null)
				return BadRequest("Invalid unit data.");

			//var headOfUnit =await _unitOfWork.Employees.GetByIdAsync(e => e.EmployeeId == unitDto.Head_Of_UnitId);
			//if (headOfUnit == null)
			//    return BadRequest("Head of unit not found.");

			//var employees =await _unitOfWork.Employees.GetAllAsync(e => unitDto.EmployeeIds.Contains(e.EmployeeId));
			//if (employees == null )
			//    return BadRequest("Employees not found.");

			var unit = new Unit()
			{
				Name = unitDto.Name,
				ArabicDescription = unitDto.ArabicDescription,
				ArabicName = unitDto.ArabicName,
				Description = unitDto.Description,

			};

			await _unitOfWork.Units.AddAsync(unit);

			int res = await _unitOfWork.CompleteAsync();
			if (res > 0)
				return Ok(true);
			return BadRequest("Failed to create unit.");


			//return CreatedAtAction(nameof(GetById), new { id = createdUnit.UnitId }, createdUnit);
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

		// PUT: api/Unit/
		[Authorize(Roles = "Admin")]
		[HttpPut("{id}")]
		public async Task<ActionResult<Unit?>> Update(int id, [FromBody] UnitUpdateDto unitDto)
		{

			var headOfUnit = await _unitOfWork.Employees.GetByIdAsync(e => e.EmployeeId == id);
			if (headOfUnit == null)
				return BadRequest("Head of unit not found.");



			//var employees = await _unitOfWork.Employees.GetAllAsync(e => unitDto.EmployeeIds.Contains(e.EmployeeId));
			//if (employees == null)
			//    return BadRequest("Employees not found.");



			var unit = await _unitOfWork.Units.GetByIdAsync(u => u.UnitId == unitDto.UnitId);
			if (unit == null)
				return NotFound("Unit not found");


			unit.UnitId = unitDto.UnitId;
			unit.Name = unitDto.Name;
			unit.ArabicName = unitDto.ArabicName;
			unit.ArabicDescription = unitDto.ArabicDescription;
			unit.Description = unitDto.Description;




			_unitOfWork.Units.Update(unit);

			int res = await _unitOfWork.CompleteAsync();
			if (res > 0)
				return Ok(unit);
			return BadRequest("Unit Update operation failed");

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
		[HttpPost("/Add_Emloyee_To_Unit/{unitId}")]
		public async Task<ActionResult<bool>> AddEmployeeToUnit(int unitId, AddEmployeeToUnitDto input)
		{
			var unit = await _unitOfWork.Units.GetByIdAsync(x => x.UnitId == unitId);
			if (unit == null) return BadRequest("No Unit Created ");
			var employee = new UnitEmployees
			{
				UnitId = unitId,
				ArabicName = input.ArabicName,
				Name = input.Name,
				ArabicJob_Title = input.ArabicJob_Title,
				Job_Title = input.Job_Title,
				Unit = unit,
			};
			if (FileMangment.UploadFile(input.Resume) == null)
				return BadRequest("Extention Or Size Not Valid For Cv");
			employee.Resume = FileMangment.UploadFile(input.Resume);
			await _unitOfWork.UnitEmployees.AddAsync(employee);
			int res = await _unitOfWork.CompleteAsync();
			if (res > 0)
				return Ok(true);
			return BadRequest("Failed to create Employee.");
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
		[HttpPost("/Add_Course_To_Unit/{unitId}")]
		public async Task<ActionResult<bool>> AddCourseToUnit(int unitId, AddCourseToUnitDto input)
		{
			var unit = await _unitOfWork.Units.GetByIdAsync(x => x.UnitId == unitId);
			if (unit == null) return BadRequest("No Unit Created ");
			var course = new UnitCourses
			{
				UnitId = unitId,
				ArabicTitle=input.ArabicTitle,
				Title=input.Title,
				unit=unit,
			};
			if (FileMangment.UploadFile(input.PdfDescription) == null)
				return BadRequest("Extention Or Size Not Valid For Cv");
			course.PdfDescription = FileMangment.UploadFile(input.PdfDescription);
			await _unitOfWork.UnitCourses.AddAsync(course);
			int res = await _unitOfWork.CompleteAsync();
			if (res > 0)
				return Ok(true);
			return BadRequest("Failed to create Employee.");
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
			var unit = await _unitOfWork.Units.GetByIdAsync(x => x.UnitId == unitId, new[] { "UnitEmployees" });
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
