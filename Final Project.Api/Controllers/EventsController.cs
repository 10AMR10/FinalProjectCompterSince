using FinalProject.Api.Helpers;
using FinalProject.Core;
using FinalProject.Core.Dtos.EmployeeDtos;
using FinalProject.Core.Dtos.EventDtos;
using FinalProject.Core.Models;
using FinalProject.EF.Translation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EventsController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IConfiguration _configuration;
		private readonly TranslationService _translationService;
		
		public EventsController(IUnitOfWork unitOfWork,IConfiguration configuration, TranslationService translationService)
		{
			_unitOfWork = unitOfWork;
			this._configuration = configuration;
			this._translationService = translationService;
		}
		[Authorize(Roles = "Admin")]
		[HttpPost("/Create_Event/{lang}")]
		public async Task<ActionResult<bool>> Create(string lang,CreateEventDto input)
		{
			if (lang== "eng")
			{
				var evnt = new Event()
				{
					Name = input.Name,
					ArabicName = await _translationService.TranslateLongTextAsync(input.Name, "en", "ar"),
					Description = input.Description,
					ArabicDescription = await _translationService.TranslateLongTextAsync(input.Description, "en", "ar"),
					Event_Start_Date = input.Event_Start_Date,


				};

				evnt.img = FileMangment.UploadFile(input.Image, _configuration);
				if (evnt.img == null)
					return BadRequest("Extention Or Size Not Valid");

				await _unitOfWork.Events.AddAsync(evnt);

				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
					return Ok(true);
				return BadRequest("Created Failed");
			}
			else
			{
				var evnt = new Event()
				{
					ArabicName = input.Name,
					Name = await _translationService.TranslateLongTextAsync(input.Name, "ar", "en"),
					ArabicDescription = input.Description,
					Description = await _translationService.TranslateLongTextAsync(input.Description, "ar", "en"),
					Event_Start_Date = input.Event_Start_Date,


				};

				evnt.img = FileMangment.UploadFile(input.Image, _configuration);
				if (evnt.img == null)
					return BadRequest("الحجم او الاضافه غير صحيح");

				await _unitOfWork.Events.AddAsync(evnt);

				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
					return Ok(true);
				return BadRequest("لم يتم عمل الحدث");
			}
		}

		[HttpGet("/Get_Event_By_Id/{id}/{lang}")]
		public async Task<ActionResult<EventDto>> Get(int id,string lang)
		{
			var evnt = await _unitOfWork.Events.GetByIdAsync(e => e.EventId == id);
			if (evnt == null)
				return NotFound("News not found");
			if (lang == "eng")
			{
				var mapped = new EventDto
				{
					Name = evnt.Name,
					Description = evnt.Description,
					Event_Start_Date = evnt.Event_Start_Date,
					EventId = evnt.EventId,
					img = evnt.img,
				};
				return Ok(mapped);
			}
			else
			{
				var mapped = new EventDto
				{
					Name = evnt.ArabicName,
					Description = evnt.ArabicDescription,
					Event_Start_Date = evnt.Event_Start_Date,
					EventId = evnt.EventId,
					img = evnt.img,
				};
				return Ok(mapped);
			}
		}

		[HttpGet("/Get_All_Events/{lang}")]
		public async Task<ActionResult<IEnumerable<EventDto>>> GetAll(string lang)
		{
			var Events = await _unitOfWork.Events.GetAllAsync(null);
			if (Events == null) return NotFound("There is no news created");
			if(lang == "eng")
			{
				var mapped = Events.Select(x => new EventDto
				{
					Name = x.Name,
					Description = x.Description,
					Event_Start_Date = x.Event_Start_Date,
					EventId = x.EventId,
					img = x.img,
				});
				return Ok(mapped);
			}
			else
			{
				var mapped = Events.Select(x => new EventDto
				{
					Name = x.ArabicName,
					Description = x.ArabicDescription,
					Event_Start_Date = x.Event_Start_Date,
					EventId = x.EventId,
					img = x.img,
				});
				return Ok(mapped);
			}
		}
		[Authorize(Roles = "Admin")]
		[HttpPut("/Update_Event/{id}/{lang}")]
		public async Task<ActionResult<bool>> Update(int id,string lang,[FromForm] CreateEventDto input)
		{
			if (lang == "eng")
			{
				var evnt = await _unitOfWork.Events.GetByIdAsync(e => e.EventId == id);

				if (evnt == null) return NotFound("Event Not Found");



				evnt.Name = input.Name;
				evnt.ArabicName = await _translationService.TranslateLongTextAsync(input.Name, "en", "ar");
				evnt.Description = input.Description;
				evnt.Description = await _translationService.TranslateLongTextAsync(input.Description, "en", "ar");
				evnt.Event_Start_Date = input.Event_Start_Date;

				evnt.img = FileMangment.UploadFile(input.Image, _configuration);
				if (evnt.img == null)
					return BadRequest("Extention Or Size Not Valid");

				_unitOfWork.Events.Update(evnt);
				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
					return Ok(true);
				return BadRequest("Event Update operation failed");

			}
			else
			{
				var evnt = await _unitOfWork.Events.GetByIdAsync(e => e.EventId == id);

				if (evnt == null) return NotFound("الحدث غير موجود");



				evnt.ArabicName = input.Name;
				evnt.Name = await _translationService.TranslateLongTextAsync(input.Name, "ar", "en");
				evnt.ArabicDescription = input.Description;
				evnt.Description = await _translationService.TranslateLongTextAsync(input.Description, "ar", "en");
				evnt.Event_Start_Date = input.Event_Start_Date;

				evnt.img = FileMangment.UploadFile(input.Image, _configuration);
				if (evnt.img == null)
					return BadRequest("الحجم او الاضافه غير صحيح");

				_unitOfWork.Events.Update(evnt);
				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
					return Ok(true);
				return BadRequest("لم يتم تعديل الحدث");

			}
		}
		[Authorize(Roles = "Admin")]
		[HttpDelete("/Delete_Event/{id}")]
		public async Task<ActionResult<bool>> Delete(int id)
		{
			var evnt = await _unitOfWork.Events.GetByIdAsync(e => e.EventId == id);
			if (evnt == null)
				return NotFound("Event Not Found");

			_unitOfWork.Events.Delete(evnt);
			
			int res = await _unitOfWork.CompleteAsync();
			if (res > 0)
				return Ok(true);
			return BadRequest("Event Delete operation failed");
		}
		[HttpGet("/Get_Latest4_Events/{lang}")]
		public async Task<ActionResult<IEnumerable<EventDto>>> GetLatestFour(string lang)
		{
			var events = await _unitOfWork.Events.GetAllAsync(null);
			if (events == null) return NotFound("There is no Events created");
			var latestFour = events.OrderByDescending(x => x.Event_Start_Date).Take(4).ToList();
			if (lang == "eng")
			{
				var mapped = latestFour.Select(x => new EventDto
				{
					EventId = x.EventId,
					Event_Start_Date = x.Event_Start_Date,
					Description = x.Description,
					img=x.img,
					Name = x.Name,
				});
				return Ok(mapped);
			}
			else
			{
				var mapped = latestFour.Select(x => new EventDto
				{
					EventId = x.EventId,
					Event_Start_Date = x.Event_Start_Date,
					Description = x.ArabicDescription,
					img = x.img,
					Name = x.ArabicName,
				});
				return Ok(mapped);
			}
		}
	}
}
