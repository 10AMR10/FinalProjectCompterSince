using FinalProject.Api.Helpers;
using FinalProject.Core;
using FinalProject.Core.Dtos.CourseDots;
using FinalProject.Core.Dtos.CourseDtos;
using FinalProject.Core.Dtos.DepartmentDtos;
using FinalProject.Core.Models;
using FinalProject.EF.Translation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace FinalProject.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CourseController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IConfiguration _configuration;
		private readonly TranslationService _translationService;

		public CourseController(IUnitOfWork unitOfWork, IConfiguration configuration, TranslationService translationService)
		{
			_unitOfWork = unitOfWork;
			this._configuration = configuration;
			this._translationService = translationService;
		}

		// POST: api/Course/Create_Course
		[Authorize(Roles = "Admin")]
		[HttpPost("Create_Course/{lang}")]
		public async Task<ActionResult<bool>> Create(string lang, [FromForm] CreateCourseDto courseDto)
		{

			if (lang == "eng")
			{
				Course course = new Course()
				{
					Title = courseDto.Title,
					ArabicTitle = await _translationService.TranslateLongTextAsync(courseDto.Title, "en", "ar"),
					LevelYearId = courseDto.LevelYearId,
					levelYear = await _unitOfWork.LevelYears.GetByIdAsync(x=> x.Id==courseDto.LevelYearId),
				};
				if (courseDto.DepartmentId is not null)
				{
					course.DepartmentId = courseDto.DepartmentId;
					course.Department = await _unitOfWork.Departments?.GetByIdAsync(d => d.DepartmentId == courseDto.DepartmentId);

				}
				var PdfDescription = FileMangment.UploadFile(courseDto.PdfDescription, _configuration);
				if (PdfDescription == null)
					return BadRequest("Extention Or Size Not Valid");
				course.PdfDescription = PdfDescription;
				await _unitOfWork.Courses.AddAsync(course);

				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
					return Ok(true);
				return BadRequest("Course Create operation failed");
			}
			else
			{
				Course course = new Course()
				{
					ArabicTitle = courseDto.Title,
					Title = await _translationService.TranslateLongTextAsync(courseDto.Title, "ar", "en"),
					LevelYearId = courseDto.LevelYearId,
					levelYear = await _unitOfWork.LevelYears.GetByIdAsync(x => x.Id == courseDto.LevelYearId),
				};
				if (courseDto.DepartmentId is not null)
				{
					course.DepartmentId = courseDto.DepartmentId;
					course.Department = await _unitOfWork.Departments?.GetByIdAsync(d => d.DepartmentId == courseDto.DepartmentId);

				}
				var PdfDescription = FileMangment.UploadFile(courseDto.PdfDescription, _configuration);
				if (PdfDescription == null)
					return BadRequest("الحجم او الاضافه غير صحيح");
				course.PdfDescription = PdfDescription;

				await _unitOfWork.Courses.AddAsync(course);

				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
					return Ok(true);
				return BadRequest("لم يتم انشاء الكورس");
			}


		}

		// GET: api/Course/Get_Course_By_Id/{id}
		[HttpGet("/Get_Course_By_Id/{id}/{lang}")]
		public async Task<ActionResult<CourseDto>> Get(int id, string lang)
		{
			if (lang == "eng")
			{
				Course course = await _unitOfWork.Courses.GetByIdAsync(c => c.CourseId == id, new[] { "Department", "levelYear" });
				if (course == null)
					return NotFound("Course not found");
				var mapped = new CourseDto
				{
					CourseId = id,
					LevelYear = course.levelYear is null ? "Non":course.levelYear.Name ,
					DepartmentId = course.Department is null ? null : course.DepartmentId,
					DepartmentName = course.Department is null ? "Non" : course.Department.Name,
					PdfDescription = course.PdfDescription,
					Title = course.Title,
				};
				return Ok(mapped);
			}
			else
			{
				Course course = await _unitOfWork.Courses.GetByIdAsync(c => c.CourseId == id, new[] { "Department" });
				if (course == null)
					return NotFound("Course not found");
				var mapped = new CourseDto
				{
					CourseId = id,
					LevelYear = course.levelYear is null ? "لا يوجد" : course.levelYear.ArabicName,
					DepartmentId = course.Department is null ? null : course.DepartmentId,
					DepartmentName = course.Department is null ? "لا يوجد" : course.Department.ArabicName,
					PdfDescription = course.PdfDescription,
					Title = course.ArabicTitle,
				};
				return Ok(mapped);
			}
		}

		// GET: api/Course/Get_All_Courses
		[HttpGet("/Get_All_Courses/{levelYear}/{lang}")]
		public async Task<ActionResult<IEnumerable<CourseDto>>> GetAll(string levelYear, string lang, [FromQuery] int? departmentId)
		{
			if (lang == "eng")
			{
				var courses = await _unitOfWork.Courses.GetAllAsync(x => x.levelYear.Name == levelYear, new[] { "Department", "levelYear" });
				if (courses == null) return NotFound("There is no course created yet.");
				IEnumerable<CourseDto> mapped = new List<CourseDto>();

				if (departmentId is null)
				{
					mapped = courses.Select(x => new CourseDto
					{
						CourseId = x.CourseId,
						LevelYear = x.levelYear?.Name,
						PdfDescription = x.PdfDescription,

						DepartmentId=x.DepartmentId,
						DepartmentName = x.Department?.Name,
						
						Title = x.Title,
					});
				}
				else
				{
					mapped = courses.Where(x => x.DepartmentId == departmentId).Select(x => new CourseDto
					{
						CourseId = x.CourseId,
						LevelYear = x.levelYear?.Name,
						PdfDescription = x.PdfDescription,
						
						
						DepartmentName = "Non",
						Title = x.Title,
					});
				}

				return Ok(mapped);

			}
			else
			{
				var courses = await _unitOfWork.Courses.GetAllAsync(x => x.levelYear.ArabicName == levelYear, new[] { "Department", "levelYear" });
				if (courses == null) return NotFound("لا يوجد كورسات");
				IEnumerable<CourseDto> mapped = new List<CourseDto>();

				if (departmentId is null)
				{
					mapped = courses.Select(x => new CourseDto
					{
						CourseId = x.CourseId,
						LevelYear = x.levelYear?.ArabicName,
						PdfDescription = x.PdfDescription,
						DepartmentId=x.DepartmentId,
						DepartmentName = x.Department?.ArabicName,
						Title = x.ArabicTitle,
					});
				}
				else
				{
					mapped = courses.Where(x => x.DepartmentId == departmentId).Select(x => new CourseDto
					{
						CourseId = x.CourseId,
						LevelYear = x.levelYear?.ArabicName,
						PdfDescription = x.PdfDescription,
						DepartmentName = "لأ يوجد",
						Title = x.ArabicTitle,
					});
				}

				return Ok(mapped);
			}

		}
		[HttpGet("Get_All_Courses/{lang}")]
		public async Task<ActionResult<IEnumerable<CourseDto>>> GetAll(string lang)
		{
			var courses = await _unitOfWork.Courses.GetAllAsync(null, new[] { "Department", "levelYear" });
			if (lang == "eng")
			{
				if (courses == null) return NotFound("There is no course created yet.");
				var mapped = courses.Select(x => new CourseDto
				{
					CourseId = x.CourseId,
					LevelYear = x.levelYear?.Name,
					PdfDescription = x.PdfDescription,
					DepartmentId = x.DepartmentId,
					DepartmentName = x.Department is null ? "Non" : x.Department.Name,
					Title = x.Title,
				});
				return Ok(mapped);

			}
			else
			{
				if (courses == null) return NotFound("لا يوجد كورسات");
				var mapped = courses.Select(x => new CourseDto
				{
					CourseId = x.CourseId,
					LevelYear = x.levelYear?.ArabicName,
					PdfDescription = x.PdfDescription,
					DepartmentId= x.DepartmentId,
					DepartmentName = x.Department is null ? "Non" : x.Department.ArabicName,
					Title = x.ArabicTitle,
				});
				return Ok(mapped);
			}


		}

		// PUT: api/Course/Update_Course
		[Authorize(Roles = "Admin")]
		[HttpPut("/Update_Course/{id}/{lang}")]
		public async Task<ActionResult<bool>> Update(int id, string lang, [FromForm] CreateCourseDto courseDto)
		{

			if (lang == "eng")
			{
				var course = await _unitOfWork.Courses.GetByIdAsync(c => c.CourseId == id, new[] { "Department" });
				if (course == null) return NotFound("Course not found");

				

				course.ArabicTitle = await _translationService.TranslateLongTextAsync(courseDto.Title, "en", "ar");
				course.Title = courseDto.Title;
				course.levelYear = await _unitOfWork.LevelYears.GetByIdAsync(x => x.Id == courseDto.LevelYearId);
				
				course.DepartmentId = courseDto.DepartmentId is null ?null : courseDto.DepartmentId;
				
				course.PdfDescription = FileMangment.UploadFile(courseDto.PdfDescription, _configuration);
				if (course.PdfDescription == null)
					return BadRequest("Extention Or Size Not Valid");

				_unitOfWork.Courses.Update(course);

				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
					return Ok(true);
				return BadRequest("Course Update operation failed");
			}
			else
			{
				var course = await _unitOfWork.Courses.GetByIdAsync(c => c.CourseId == id, new[] { "Department" });
				if (course == null) return NotFound("الكورس غيرموجود");

				

				course.Title = await _translationService.TranslateLongTextAsync(courseDto.Title, "ar", "en");
				course.ArabicTitle = courseDto.Title;
				
				course.levelYear = await _unitOfWork.LevelYears.GetByIdAsync(x => x.Id == courseDto.LevelYearId);
				course.DepartmentId = courseDto.DepartmentId;
				
				course.PdfDescription = FileMangment.UploadFile(courseDto.PdfDescription, _configuration);
				if (course.PdfDescription == null)
					return BadRequest("الحجم او الاضافه غير صحيح");

				_unitOfWork.Courses.Update(course);

				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
					return Ok(true);
				return BadRequest("لم يتم الكورس");
			}

		}

		// DELETE: api/Course/Delete_Course/{id}
		[Authorize(Roles = "Admin")]
		[HttpDelete("/Delete_Course/{id}")]
		public async Task<ActionResult<bool>> Delete(int id)
		{
			Course course = await _unitOfWork.Courses.GetByIdAsync(c => c.CourseId == id, new[] { "Department" });
			if (course == null) return NotFound("Course Not Found");

			_unitOfWork.Courses.Delete(course);

			int res = await _unitOfWork.CompleteAsync();
			if (res > 0)
				return Ok(true);
			return BadRequest("Course Delete operation failed");
		}
	}
}
