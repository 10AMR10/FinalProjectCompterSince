using FinalProject.Api.Helpers;
using FinalProject.Core;
using FinalProject.Core.Dtos.CategoryDtos;
using FinalProject.Core.Dtos.CourseDots;
using FinalProject.Core.Dtos.CourseDtos;
using FinalProject.Core.Models;
using FinalProject.EF.Translation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FinalProject.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoryController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly TranslationService _translationService;

		public CategoryController(IUnitOfWork unitOfWork, TranslationService translationService)
		{
			_unitOfWork = unitOfWork;
			this._translationService = translationService;
		}
		//[Authorize(Roles = "Admin")]
		[HttpPost("{lang}")]
		public async Task<ActionResult<bool>> Create(string lang,[FromForm] CreateCategoryDto input)
		{
			if (lang=="eng")
			{
				var category = new Category
				{
					ArabicName = await _translationService.TranslateLongTextAsync(input.Name, "en", "ar"),
					Name = input.Name,
				};

				await _unitOfWork.Categories.AddAsync(category);

				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
					return Ok(true);
				return BadRequest("Course category operation failed");
			}
			else
			{
				var category = new Category
				{
					Name = await _translationService.TranslateLongTextAsync(input.Name, "ar", "en"),
					ArabicName = input.Name,
				};

				await _unitOfWork.Categories.AddAsync(category);

				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
					return Ok(true);
				return BadRequest("لم يتم عمل فئه");
			}
		}
		[HttpGet("All/{lang}")]
		public async Task<ActionResult<IReadOnlyList<CategoryDto>>> GetAll(string lang)
		{
			var categories = await _unitOfWork.Categories.GetAllAsync(null);
			if (lang == "eng")
			{
				if (categories == null)
					return NotFound("Categories Not Found");
				var mapped = categories.Select(x => new CategoryDto
				{
					Id = x.Id,
					Name = x.Name,
				});
				return Ok(mapped);
			}
			else
			{
				if (categories == null)
					return NotFound("لا يوجد فئات");
				var mapped = categories.Select(  x => new CategoryDto
				{
					Id = x.Id,
					Name = x.ArabicName,
				});
				return Ok(mapped);
			}
		}

		[HttpGet("{id}/{lang}")]
		public async Task<ActionResult<CategoryDto>> Get(int id, string lang)
		{
			var category = await _unitOfWork.Categories.GetByIdAsync(x => x.Id == id);
			if (lang == "eng")
			{
				if (category == null)
					return NotFound("Category Not Found");
				var mapped = new CategoryDto
				{
					Id = category.Id,
					Name = category.Name,
				};
				return Ok(mapped);
			}
			else
			{
				if (category == null)
					return NotFound("لا يوجد فئات");
				var mapped = new CategoryDto
				{
					Id = category.Id,
					Name = category.ArabicName
				};
				return Ok(mapped);
			}
		}

		[Authorize(Roles = "Admin")]
		[HttpPut("{id}/{lang}")]
		public async Task<ActionResult<bool>> Update(int id,string lang, [FromBody] UpdateCategoryDto input)
		{
			if (lang=="eng")
			{
				var category = await _unitOfWork.Categories.GetByIdAsync(x => x.Id == id);
				if (category == null)
					return NotFound("Category Not Found");
				category.Name = input.Name;
				category.ArabicName = await _translationService.TranslateLongTextAsync(input.Name, "en", "ar");
				_unitOfWork.Categories.Update(category);

				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
					return Ok(true);
				return BadRequest("category Update operation failed"); 
			}
			else
			{
				var category = await _unitOfWork.Categories.GetByIdAsync(x => x.Id == id);
				if (category == null)
					return NotFound("Category Not Found");
				category.Name = await _translationService.TranslateLongTextAsync(input.Name, "ar", "en");
				category.ArabicName = input.Name;
				_unitOfWork.Categories.Update(category);

				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
					return Ok(true);
				return BadRequest("category Update operation failed");
			}
		}
		[Authorize(Roles = "Admin")]
		[HttpDelete("{id}")]
		public async Task<ActionResult<bool>> Delete(int id)
		{
			var category = await _unitOfWork.Categories.GetByIdAsync(x => x.Id == id);
			if (category == null)
				return NotFound("Category Not Found");
			_unitOfWork.Categories.Delete(category);

			int res = await _unitOfWork.CompleteAsync();
			if (res > 0)
				return Ok(true);
			return BadRequest("category Delete operation failed");
		}
	}
}
