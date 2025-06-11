using FinalProject.Api.Helpers;
using FinalProject.Core;
using FinalProject.Core.Dtos.CourseDots;
using FinalProject.Core.Dtos.NewsDtos;
using FinalProject.Core.Models;
using FinalProject.EF.Translation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
namespace FinalProject.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class NewsController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IConfiguration _configuration;
		private readonly TranslationService _translationService;

		public NewsController(IUnitOfWork unitOfWork, IConfiguration configuration, TranslationService translationService)
		{
			_unitOfWork = unitOfWork;
			this._configuration = configuration;
			this._translationService = translationService;
		}
		//[Authorize(Roles = "Admin")]
		[HttpPost("{lang}")]
		public async Task<ActionResult<bool>> Create(string lang,[FromForm] CreateNewsDto input)
		{
			if (lang == "eng")
			{
				News news = new News()
				{
					Name = input.Name,
					ArabicDescription = await _translationService.TranslateLongTextAsync(input.Description, "en", "ar"),
					ArabicName = await _translationService.TranslateLongTextAsync(input.Name, "en", "ar"),
					Description = input.Description,
					News_Date = input.News_Date
				};
				news.img = FileMangment.UploadFile(input.Image, _configuration);
				if (news.img == null)
					return BadRequest("Extention Or Size Not Valid");


				await _unitOfWork.News.AddAsync(news);

				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
					return Ok(true);
				return BadRequest("Add News operation failed");
			}
			else
			{
				News news = new News()
				{
					ArabicName = input.Name,
					Name = await _translationService.TranslateLongTextAsync(input.Name, "ar", "en"),
					ArabicDescription = input.Description,
					Description = await _translationService.TranslateLongTextAsync(input.Description, "ar", "en"),
					News_Date = input.News_Date
				};
				news.img = FileMangment.UploadFile(input.Image, _configuration);
				if (news.img == null)
					return BadRequest("الحجم او الاضافه غير صحيح");


				await _unitOfWork.News.AddAsync(news);

				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
					return Ok(true);
				return BadRequest("لم يتم اضافه الخبر");
			}
		}

		[HttpGet("/Get_News_By_Id/{id}/{lang}")]
		public async Task<ActionResult<NewsDto>> Get(int id, string lang)
		{
			var news = await _unitOfWork.News.GetByIdAsync(n => n.NewsId == id);
			if (news == null) return NotFound("News not found");
			if (lang == "eng")
			{
				var mapped = new NewsDto
				{
					NewsId = id,
					Description = news.Description,
					img = news.img,
					Name = news.Name,
					News_Date = news.News_Date
				};
				return Ok(mapped);
			}
			else
			{
				var mapped = new NewsDto
				{
					NewsId = id,
					Description = news.ArabicDescription,
					img = news.img,
					Name = news.ArabicName,
					News_Date = news.News_Date
				};
				return Ok(mapped);
			}
		}

		[HttpGet("/Get_All_News/{lang}")]
		public async Task<ActionResult<IEnumerable<NewsDto>>> GetAll(string lang)
		{
			var news = await _unitOfWork.News.GetAllAsync(null);
			if (news.Count()==0) return NotFound("There is no news created");
			if (lang == "eng")
			{
				var mapped = news.Select(x => new NewsDto
				{
					NewsId = x.NewsId,
					Name = x.Name,
					Description = x.Description,
					img = x.img,
					News_Date = x.News_Date
				});
				return Ok(mapped);
			}
			else
			{
				var mapped = news.Select(x => new NewsDto
				{
					NewsId = x.NewsId,
					Name = x.ArabicName,
					Description = x.ArabicDescription,
					img = x.img,
					News_Date = x.News_Date
				});
				return Ok(mapped);
			}
		}
		//[Authorize(Roles = "Admin")]
		[HttpPut("/Update_News/{id}/{lang}")]
		public async Task<ActionResult<bool>> Update(int id,string lang, UpdateNewsDto input)
		{
			if (lang=="eng")
			{
				News news = await _unitOfWork.News.GetByIdAsync(n => n.NewsId == id);

				if (news == null) return NotFound("News not found");
				news.Name = input.Name;
				news.ArabicName = await _translationService.TranslateLongTextAsync(input.Name, "en", "ar");
				news.ArabicDescription = await _translationService.TranslateLongTextAsync(input.Description, "en", "ar");
				news.News_Date = input.News_Date;

				news.img = FileMangment.UploadFile(input.Image, _configuration);
				if (news.img == null)
					return BadRequest("Extention Or Size Not Valid");

				_unitOfWork.News.Update(news);

				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
					return Ok(true);
				return BadRequest("News Update operation failed");
			}
			else
			{
				News news = await _unitOfWork.News.GetByIdAsync(n => n.NewsId == id);

				if (news == null) return NotFound("الخبر غير موجود");
				news.ArabicName = input.Name;
				news.Name = await _translationService.TranslateLongTextAsync(input.Name, "ar", "en");
				news.Description = await _translationService.TranslateLongTextAsync(input.Description, "ar", "en");
				news.ArabicDescription = input.Description;
				news.News_Date = input.News_Date;

				news.img = FileMangment.UploadFile(input.Image, _configuration);
				if (news.img == null)
					return BadRequest("الحجم او الاضافه غير صحيح");

				_unitOfWork.News.Update(news);

				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
					return Ok(true);
				return BadRequest("لم يتم تعديل الخبر");
			}
		}
		[Authorize(Roles = "Admin")]
		[HttpDelete("/Delete_News/{id}")]
		public async Task<ActionResult<News>> Delete(int id)
		{
			var news = await _unitOfWork.News.GetByIdAsync(n => n.NewsId == id);
			if (news == null) return NotFound("News Not Found");

			_unitOfWork.News.Delete(news);

			int res = await _unitOfWork.CompleteAsync();
			if (res > 0)
				return Ok(true);
			return BadRequest("News delete operation failed");
		}
		[HttpGet("/Get_Latest4_News/{lang}")]
		public async Task<ActionResult<IEnumerable<NewsDto>>> GetLatestFour(string lang)
		{
			var news = await _unitOfWork.News.GetAllAsync(null);
			if (news == null) return NotFound("There is no news created");
			var latestFour=news.OrderByDescending(x=>x.News_Date).Take(4).ToList();
			if (lang == "eng")
			{
				var mapped = latestFour.Select(x => new NewsDto
				{
					NewsId = x.NewsId,
					Name = x.Name,
					Description = x.Description,
					img = x.img,
					News_Date = x.News_Date
				});
				return Ok(mapped);
			}
			else
			{
				var mapped = latestFour.Select(x => new NewsDto
				{
					NewsId = x.NewsId,
					Name = x.ArabicName,
					Description = x.ArabicDescription,
					img = x.img,
					News_Date = x.News_Date
				});
				return Ok(mapped);
			}
		}
	}
}
