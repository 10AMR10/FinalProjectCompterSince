using FinalProject.Core.Dtos.CourseDtos;
using FinalProject.Core.Models;
using FinalProject.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using FinalProject.Core.Models;
using FinalProject.Core.Dtos.CollegeDots;
using Microsoft.AspNetCore.Http;
using FinalProject.EF.Migrations;
using System.Collections.Generic;
using FinalProject.EF;
using FinalProject.Core;
using Microsoft.AspNetCore.Authorization;
using FinalProject.Api.Helpers;
using FinalProject.Core.Dtos.CourseDots;
namespace FinalProject.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CourseController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;

		public CourseController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		// POST: api/Course/Create_Course
		[Authorize(Roles = "Admin")]
		[HttpPost("/Create_Course")]
		public async Task<ActionResult<bool>> Create([FromBody] CreateCourseDto courseDto)
		{
			
				Course course = new Course()
				{
					Title = courseDto.Title,
					ArabicTitle= courseDto.ArabicTitle,
					LevelYear = courseDto.LevelYear,
					ArabicLevelYear= courseDto.ArabicLevelYear,
					DepartmentId = courseDto.DepartmentId,
					//Department =await _unitOfWork.Departments.GetByIdAsync(d => d.DepartmentId == courseDto.DepartmentId)
				};
				if (FileMangment.UploadFile(courseDto.PdfDescription) == null)
					return BadRequest("Extention Or Size Not Valid");
				course.PdfDescription = FileMangment.UploadFile(courseDto.PdfDescription);

				var AddCourse = await _unitOfWork.Courses.AddAsync(course);
				if (AddCourse == null) return BadRequest("Add Course operation failed ");

				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
					return Ok(true);
				return BadRequest("Course Create operation failed");
			

		}

		// GET: api/Course/Get_Course_By_Id/{id}
		[HttpGet("/Get_Course_By_Id/{id}/{lang}")]
		public async Task<ActionResult<CourseDto>> Get(int id, string lang)
		{
			if (lang == "eng")
			{
				Course course = await _unitOfWork.Courses.GetByIdAsync(c => c.CourseId == id, new[] { "Department" });
				if (course == null)
					return NotFound("Course not found");
				var mapped = new CourseDto
				{
					CourseId = id,
					LevelYear = course.LevelYear,
					DepartmentName = course.Department.Name,
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
					LevelYear = course.ArabicLevelYear,
					DepartmentName = course.Department.ArabicName,
					PdfDescription = course.PdfDescription,
					Title = course.ArabicTitle,
				};
				return Ok(mapped);
			}
		}

		// GET: api/Course/Get_All_Courses
		[HttpGet("/Get_All_Courses/{levelYear}/{department}/{lang}")]
		public async Task<ActionResult<IEnumerable<CourseDto>>> GetAll(string levelYear, int? departmentId, string lang)
		{
			if (lang == "eng")
			{
				var courses = await _unitOfWork.Courses.GetAllAsync(x => x.LevelYear == levelYear, new[] { "Department" });
				if (courses == null) return NotFound("There is no course created yet.");
				IEnumerable<CourseDto> mapped = new List<CourseDto>();

				if (departmentId is null)
				{
					mapped = courses.Select(x => new CourseDto
					{
						CourseId = x.CourseId,
						LevelYear = levelYear,
						PdfDescription = x.PdfDescription,
						DepartmentName = x.Department.Name,
						Title = x.Title,
					});
				}
				else
				{
					mapped = courses.Where(x => x.DepartmentId == departmentId).Select(x => new CourseDto
					{
						CourseId = x.CourseId,
						LevelYear = levelYear,
						PdfDescription = x.PdfDescription,
						DepartmentName = x.Department.Name,
						Title = x.Title,
					});
				}

				return Ok(mapped);

			}
			else
			{
				var courses = await _unitOfWork.Courses.GetAllAsync(x => x.LevelYear == levelYear, new[] { "Department" });
				if (courses == null) return NotFound("There is no course created yet.");
				IEnumerable<CourseDto> mapped = new List<CourseDto>();

				if (departmentId is null)
				{
					mapped = courses.Select(x => new CourseDto
					{
						CourseId = x.CourseId,
						LevelYear = x.LevelYear,
						PdfDescription = x.PdfDescription,
						DepartmentName = x.Department.ArabicName,
						Title = x.ArabicTitle,
					});
				}
				else
				{
					mapped = courses.Where(x => x.DepartmentId == departmentId).Select(x => new CourseDto
					{
						CourseId = x.CourseId,
						LevelYear = x.LevelYear,
						PdfDescription = x.PdfDescription,
						DepartmentName = x.Department.ArabicName,
						Title = x.ArabicTitle,
					});
				}

				return Ok(mapped);
			}

		}

		// PUT: api/Course/Update_Course
		[Authorize(Roles = "Admin")]
		[HttpPut("/Update_Course/{id}")]
		public async Task<ActionResult<bool>> Update(int id, [FromBody] CreateCourseDto courseDto)
		{
			
				var course = await _unitOfWork.Courses.GetByIdAsync(c => c.CourseId == id, new[] { "Department" });
				if (course == null) return NotFound("Course not found");

				var department = await _unitOfWork.Departments.GetByIdAsync(d => d.DepartmentId == courseDto.DepartmentId);
				if (department == null) return NotFound("Department not found");


			
			course.ArabicTitle = course.ArabicTitle;
			course.Title = courseDto.Title;
			course.LevelYear = courseDto.LevelYear;
			course.ArabicLevelYear = courseDto.LevelYear;
			course.DepartmentId = courseDto.DepartmentId;
					//Department = await _unitOfWork.Departments.GetByIdAsync(d => d.DepartmentId == courseDto.DepartmentId)

				
				if (FileMangment.UploadFile(courseDto.PdfDescription) == null)
					return BadRequest("Extention Or Size Not Valid");
				course.PdfDescription = FileMangment.UploadFile(courseDto.PdfDescription);

				_unitOfWork.Courses.Update(course);
				
				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
					return Ok(true);
				return BadRequest("Course Update operation failed");
			
		}

		// DELETE: api/Course/Delete_Course/{id}
		[Authorize(Roles = "Admin")]
		[HttpDelete("/Delete_Course/{id}")]
		public async Task<ActionResult<Course>> Delete(int id)
		{
			Course course = await _unitOfWork.Courses.GetByIdAsync(c => c.CourseId == id, new[] { "Department" });
			if (course == null) return NotFound("Course Not Found");

			 _unitOfWork.Courses.Delete(course);
			
			int res = await _unitOfWork.CompleteAsync();
			if (res > 0)
				return Ok(course);
			return BadRequest("Course Delete operation failed");
		}
	}
}
