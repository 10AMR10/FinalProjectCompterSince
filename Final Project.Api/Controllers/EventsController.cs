using FinalProject.Core.Models;
using FinalProject.Core;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using FinalProject.Core.Dtos.EmployeeDots;
using FinalProject.Core.Models;
using Microsoft.AspNetCore.Http;
using FinalProject.Core.Dtos.CollegeDots;
using System.Collections.Generic;
using FinalProject.EF;
using FinalProject.Core;
using FinalProject.Core.Dtos.EventDtos;
using FinalProject.EF.Migrations;
using Microsoft.AspNetCore.Authorization;
using FinalProject.Api.Helpers;
using FinalProject.Core.Dtos.NewsDtos;

namespace FinalProject.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EventsController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;

		public EventsController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		[Authorize(Roles = "Admin")]
		[HttpPost("/Create_Event")]
		public async Task<ActionResult<bool>> Create(CreateEvent EventDto)
		{
			var evnt = new Event()
			{
				Name = EventDto.Name,
				ArabicName = EventDto.ArabicName,
				Description = EventDto.Description,
				ArabicDescription = EventDto.ArabicDescription,
				Event_Start_Date = EventDto.Event_Start_Date,


			};

			if (FileMangment.UploadFile(EventDto.Image) == null)
				return BadRequest("Extention Or Size Not Valid");
			evnt.img = FileMangment.UploadFile(EventDto.Image);

			await _unitOfWork.Events.AddAsync(evnt);

			int res = await _unitOfWork.CompleteAsync();
			if (res > 0)
				return Ok(true);
			return BadRequest("Created Failed");
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
		[HttpPut("/Update_Event/{id}")]
		public async Task<ActionResult<bool>> Update(int id,[FromBody] UpdateEvent EventDto)
		{
			var evnt = await _unitOfWork.Events.GetByIdAsync(e => e.EventId == EventDto.EventId, new[] { "College" });

			if (evnt == null) return NotFound("Event Not Found");


			evnt.EventId = EventDto.EventId;
			evnt.Name = EventDto.Name;
			evnt.ArabicName = EventDto.ArabicName;
			evnt.Description = EventDto.Description;
			evnt.Description = EventDto.ArabicDescription;
			evnt.Event_Start_Date = EventDto.Event_Start_Date;
				
			if (FileMangment.UploadFile(EventDto.Image) == null)
				return BadRequest("Extention Or Size Not Valid");
			evnt.img = FileMangment.UploadFile(EventDto.Image);

			_unitOfWork.Events.Update(evnt);
			int res = await _unitOfWork.CompleteAsync();
			if (res > 0)
				return Ok(true);
			return BadRequest("Event Update operation failed");

		}
		[Authorize(Roles = "Admin")]
		[HttpDelete("/Delete_Event/{id}")]
		public async Task<ActionResult<bool>> Delete(int id)
		{
			var evnt = await _unitOfWork.Events.GetByIdAsync(e => e.EventId == id, new[] { "College" });
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
					Description = x.Description,
					img = x.img,
					Name = x.Name,
				});
				return Ok(mapped);
			}
		}
	}
}
