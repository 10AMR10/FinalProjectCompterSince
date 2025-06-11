using FinalProject.Core;
using FinalProject.Core.Dtos.CourseDots;
using FinalProject.Core.Models;
using FinalProject.EF.Translation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LevelYearController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
	

		public LevelYearController(IUnitOfWork unitOfWork, TranslationService translationService)
		{
			_unitOfWork = unitOfWork;
			
		}

		[HttpPost("{name}/{arbicName}")]
		public async Task<ActionResult<bool>> Create(string name,string arbicName)
		{
			var level = new LevelYear()
			{
				Name = name,
				ArabicName = arbicName
			};
			await _unitOfWork.LevelYears.AddAsync(level);
			var res=await _unitOfWork.CompleteAsync();
			if(res >0)
				return Ok(true);
			return BadRequest("can't create it");

		}
		[HttpGet("{lang}")]
		public async Task<ActionResult<IEnumerable<LevelYearDto>>> GetAll(string lang)
		{
			var levels = await _unitOfWork.LevelYears.GetAllAsync(null);
			if (lang == "eng")
			{
				var mapped = levels.Select(x => new LevelYearDto
				{
					Id = x.Id,
					Name = x.Name,
				});
				return Ok(mapped);
			}
			else
			{
				var mapped = levels.Select(x => new LevelYearDto
				{
					Id = x.Id,
					Name = x.ArabicName,
				});
				return Ok(mapped);
			}
		}
	}
}
