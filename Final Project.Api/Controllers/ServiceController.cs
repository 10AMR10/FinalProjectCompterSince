using FinalProject.Api.Helpers;
using FinalProject.Core;
using FinalProject.Core.Dtos.CategoryDtos;
using FinalProject.Core.Dtos.CourseDots;
using FinalProject.Core.Dtos.ServiceDtos;
using FinalProject.Core.Models;
using FinalProject.EF.Translation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ServiceController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IConfiguration _configuration;
		private readonly TranslationService _translationService;

		public ServiceController(IUnitOfWork unitOfWork, IConfiguration configuration, TranslationService translationService)
		{
			_unitOfWork = unitOfWork;
			this._configuration = configuration;
			this._translationService = translationService;
		}

		[Authorize(Roles = "Admin")]
		[HttpPost("{categoryId}/{lang}")]
		public async Task<ActionResult<bool>> Create(int categoryId,string lang,[FromForm] CreateServiceDto input)
		{
			if (lang == "eng")
			{
				var service = new Service
				{
					Title = input.Title,
					ArabicTitle = await _translationService.TranslateLongTextAsync(input.Title, "en", "ar"),
					categoryId = categoryId,
				};
				var category = await _unitOfWork.Categories.GetByIdAsync(x => x.Id == categoryId);
				if (category == null)
					return BadRequest("category Not Created ");
				service.category = category;
				category?.Services?.Add(service);
				service.PdfDescription = FileMangment.UploadFile(input.PdfDescription, _configuration);
				if (service.PdfDescription == null)
					return BadRequest("Extention Or Size Not Valid");
				await _unitOfWork.Services.AddAsync(service);

				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
					return Ok(true);
				return BadRequest("service Create operation failed");
			}
			else
			{
				var service = new Service
				{
					ArabicTitle = input.Title,
					Title = await _translationService.TranslateLongTextAsync(input.Title, "ar", "en"),
					categoryId = categoryId,
				};
				var category = await _unitOfWork.Categories.GetByIdAsync(x => x.Id == categoryId);
				if (category == null)
					return BadRequest("الفئه غير موجوده");
				service.category = category;
				category?.Services?.Add(service);
				service.PdfDescription = FileMangment.UploadFile(input.PdfDescription, _configuration);
				if (service.PdfDescription == null)
					return BadRequest("الحجم او الاضافه غير صحيح");
				await _unitOfWork.Services.AddAsync(service);

				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
					return Ok(true);
				return BadRequest("لم يتم عمل الخدمه");
			}
		}
		[HttpGet("All/{categoryId}/{lang}")]
		public async Task<ActionResult<IReadOnlyList<ServiceDto>>> GetAll(int categoryId,string lang)
		{
			var category = await _unitOfWork.Categories.GetByIdAsync(x => x.Id == categoryId, new[] { "Services" });
			if (lang == "eng")
			{
				if (category == null) return BadRequest("Category Not Created");
				var mapped = category.Services.Select(x => new ServiceDto
				{
					Id = x.Id,
					Title = x.Title,
					PdfDescription = x.PdfDescription,
				});
				
				return Ok(mapped);
			}
			else
			{
				if (category == null) return BadRequest("لم يتم انشاء الفئه");
				var mapped = category.Services?.Select(x => new ServiceDto
				{
					Id = x.Id,
					Title = x.ArabicTitle,
					PdfDescription = x.PdfDescription,
				});
				return Ok(mapped);
			}
		}

		[HttpGet("All_Detials/{lang}")]
		public async Task<ActionResult<IReadOnlyList<ServiceCategoryDto>>> GetAllDetails(string lang)
		{
			var services = await _unitOfWork.Services.GetAllAsync(null, new[] { "category" });
			if (lang == "eng")
			{
				if (services == null) return BadRequest("Services Not Created");
				var mapped = services.Select(x => new ServiceCategoryDto
				{
					Id = x.Id,
					Title = x.Title,
					PdfDescription = x.PdfDescription,
					CategoryId=x.category.Id,
					CategoryName=x.category.Name,


				});

				return Ok(mapped);
			}
			else
			{
				if (services == null) return BadRequest("لم يتم انشاء الخدمه");
				var mapped = services.Select(x => new ServiceCategoryDto
				{
					Id = x.Id,
					Title = x.ArabicTitle,
					PdfDescription = x.PdfDescription,
					CategoryId = x.category.Id,
					CategoryName = x.category.ArabicName,


				});
				return Ok(mapped);
			}
		}

		[HttpGet("{id}/{lang}")]
		public async Task<ActionResult<ServiceDto>> Get(int id, string lang)
		{
			var service = await _unitOfWork.Services.GetByIdAsync(x => x.Id == id);
			if (lang == "eng")
			{
				if (service == null)
					return NotFound("Service Not Found");
				var mapped = new ServiceDto
				{
					Id = service.Id,
					Title= service.Title,
					PdfDescription = service.PdfDescription,
				};
				return Ok(mapped);
			}
			else
			{
				if (service == null)
					return NotFound("لا يوجد خدمه");
				var mapped = new ServiceDto
				{
					Id = service.Id,
					Title=service.ArabicTitle,
					PdfDescription = service.PdfDescription,
				};
				return Ok(mapped);
			}
		}

		[Authorize(Roles = "Admin")]
		[HttpPut("{id}/{categoryId}/{lang}")]
		public async Task<ActionResult<bool>> Update(int id, int categoryId, string lang, [FromForm] CreateServiceDto input)
		{
			if (lang=="eng")
			{
				var service = await _unitOfWork.Services.GetByIdAsync(x => x.Id == id);
				if (service == null)
					return NotFound("Service Not Found");
				service.Title = input.Title;
				service.ArabicTitle = await _translationService.TranslateLongTextAsync(input.Title, "en", "ar");
				service.categoryId= categoryId;
				service.PdfDescription = FileMangment.UploadFile(input.PdfDescription, _configuration);
				if (service.PdfDescription == null)
					return BadRequest("Extention Or Size Not Valid");

				_unitOfWork.Services.Update(service);

				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
					return Ok(true);
				return BadRequest("service Update operation failed");
			}
			else
			{
				var service = await _unitOfWork.Services.GetByIdAsync(x => x.Id == id);
				if (service == null)
					return NotFound("الخدمه غير موجوده");
				service.ArabicTitle = input.Title;
				service.Title = await _translationService.TranslateLongTextAsync(input.Title, "ar", "en");
				service.categoryId = categoryId;

				service.PdfDescription = FileMangment.UploadFile(input.PdfDescription, _configuration);
				if (service.PdfDescription == null)
					return BadRequest("الحجم او الاضافه غير صحيح");

				_unitOfWork.Services.Update(service);

				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
					return Ok(true);
				return BadRequest("لم يتم تعديل الخدمه");
			}
		}
		[Authorize(Roles = "Admin")]
		[HttpDelete("{id}")]
		public async Task<ActionResult<bool>> Delete(int id)
		{
			var service = await _unitOfWork.Services.GetByIdAsync(x => x.Id == id);
			if (service == null)
				return NotFound("Service Not Found");
			_unitOfWork.Services.Delete(service);

			int res = await _unitOfWork.CompleteAsync();
			if (res > 0)
				return Ok(true);
			return BadRequest("Service Delete operation failed");
		}
	}
}
