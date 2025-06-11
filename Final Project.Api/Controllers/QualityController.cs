using FinalProject.Core;
using FinalProject.Core.Dtos.QuailtyDtos;
using FinalProject.Core.Models;
using FinalProject.EF.Translation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace FinalProject.Api.Controllers

{
	[Route("api/[controller]")]
    [ApiController]
    public class QualityController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
		private readonly TranslationService _translationService;

		public QualityController(IUnitOfWork unitOfWork, TranslationService translationService)
        {
            _unitOfWork = unitOfWork;
			this._translationService = translationService;
		}
		[Authorize(Roles = "Admin")]
		[HttpPost("/Add_Quality/{lang}")]
        public async Task<ActionResult<bool>> Create(string lang,AddQualityDto input)
        {
            if (lang=="eng")
            {
                var Quality = new Quality()
                {
                    Name = input.Name,
                    ArabicName = await _translationService.TranslateLongTextAsync(input.Name, "en", "ar"),
                    ArabicDescription = await _translationService.TranslateLongTextAsync(input.Description, "en", "ar"),
                    Description = input.Description,
                };
                await _unitOfWork.Qualities.AddAsync(Quality);

                int res = await _unitOfWork.CompleteAsync();
                if (res > 0)
                    return Ok(Quality);
                return BadRequest("Add Quality operation failed"); 
            }
            else
            {
				var Quality = new Quality()
				{
					ArabicName = input.Name,
					Name = await _translationService.TranslateLongTextAsync(input.Name, "ar", "en"),
					Description = await _translationService.TranslateLongTextAsync(input.Description, "ar", "en"),
					ArabicDescription = input.Description,
				};
				await _unitOfWork.Qualities.AddAsync(Quality);

				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
					return Ok(Quality);
				return BadRequest("لم يتم اضافه الجوده");
			}
        }

        [HttpGet("/Get_Quality_By_Id/{id}/{lang}")]
        public async Task<ActionResult<QualityDto>> Get(int id,string lang)
        {
            var Quality = await _unitOfWork.Qualities.GetByIdAsync(e => e.Id == id);
            if (Quality == null) return NotFound("News not found");
            if (lang == "eng")
            {
                var mapped = new QualityDto
                {
                    Name = Quality.Name,
                    
                    Description = Quality.Description,
                    Id = id
                };
                return Ok(mapped);

			}
            else
            {
				var mapped = new QualityDto
				{
					Name = Quality.ArabicName,
					Description = Quality.ArabicDescription,
					Id = id
				};
				return Ok(mapped);
			}
        }

        [HttpGet("/Get_All_Qualitys/{lang}")]
        public async Task<ActionResult<IEnumerable<QualityDto>>> GetAll(string lang)
        {
            var qualitys = await _unitOfWork.Qualities.GetAllAsync(null);
            if (lang == "eng")
            {
            if (qualitys is null) return NotFound("There is no qualities created");
                var mapped = qualitys.Select(x => new QualityDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                });
                return Ok(mapped);
            }
            else
            {
            if (qualitys is null) return NotFound("لم يتم انشاء جوده");
                
                var mapped = qualitys.Select(x => new QualityDto
                {
                    Id = x.Id,
                    Name = x.ArabicName,
                    Description = x.ArabicDescription,
                });
                return Ok(mapped);
            }

        }
		[Authorize(Roles = "Admin")]
		[HttpPut("/Update_Quality/{id}/{lang}")]
        public async Task<ActionResult<bool?>> Update(int id,string lang,AddQualityDto input)
        {
            if (lang=="eng")
            {
                var quality = await _unitOfWork.Qualities.GetByIdAsync(e => e.Id == id);

                if (quality == null) return
                        NotFound("Quality Not Found");

                quality.Name = input.Name;
                quality.ArabicName = await _translationService.TranslateLongTextAsync(input.Name, "en", "ar");
                quality.ArabicDescription = await _translationService.TranslateLongTextAsync(input.Description, "en", "ar");
                quality.Description = input.Description;


                _unitOfWork.Qualities.Update(quality);

                int res = await _unitOfWork.CompleteAsync();
                if (res > 0)
                    return Ok(true);
                return BadRequest("Quality Update operation failed");
            }
            else
            {
				var quality = await _unitOfWork.Qualities.GetByIdAsync(e => e.Id == id);

				if (quality == null) return
						NotFound("الجوده غير موجوده");

				quality.ArabicName = input.Name;
				quality.Name = await _translationService.TranslateLongTextAsync(input.Name, "ar", "en");
				quality.Description = await _translationService.TranslateLongTextAsync(input.Description, "ar", "en");
				quality.ArabicDescription = input.Description;


				_unitOfWork.Qualities.Update(quality);

				int res = await _unitOfWork.CompleteAsync();
				if (res > 0)
					return Ok(true);
				return BadRequest("لم يتم تعديل الجوده");
			}
		}
		[Authorize(Roles = "Admin")]
		[HttpDelete("/Delete_Quality/{id}")]
        public async Task<ActionResult<bool?>> Delete(int id)
        {
            var quality = await _unitOfWork.Qualities.GetByIdAsync(e => e.Id == id);
            if (quality == null)
                return NotFound("Quality Not Found");

            _unitOfWork.Qualities.Delete(quality);
            
			int res = await _unitOfWork.CompleteAsync();
			if (res > 0)
				return Ok(true);
			return BadRequest("Quality Delete operation failed");
		}
    }
}
