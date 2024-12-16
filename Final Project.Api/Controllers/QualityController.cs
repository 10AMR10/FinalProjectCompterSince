using FinalProject.Core.Dtos.QuailtyDtos;
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
    using FinalProject.EF.Migrations;
using Microsoft.AspNetCore.Authorization;
namespace FinalProject.Api.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class QualityController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        public QualityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
		[Authorize(Roles = "Admin")]
		[HttpPost("/Add_Quality")]
        public async Task<ActionResult<bool>> Create(AddQualityDto QualityDto)
        {
            var Quality = new Quality()
            {
                Name = QualityDto.Name,
                ArabicDescription=QualityDto.Description,
                ArabicName=QualityDto.Name,
                Description = QualityDto.Description,
            };
            await _unitOfWork.Qualities.AddAsync(Quality);
           
			int res = await _unitOfWork.CompleteAsync();
			if (res > 0)
				return Ok(Quality);
			return BadRequest("Add Quality operation failed");
			
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
            if (qualitys is  null) return NotFound("There is no qualities created");
   //         if (lang == "eng")
   //         {
   //             var mapped = qualitys.Select(x => new QualityDto
   //             {
   //                 Id = x.Id,
   //                 Name = x.Name,
   //                 Description = x.Description,
   //             });
   //             return Ok(mapped);
   //         }
   //         else
   //         {
			//	var mapped = qualitys.Select(x => new QualityDto
			//	{
			//		Id = x.Id,
			//		Name = x.Name,
			//		Description = x.Description,
			//	});
			//	return Ok(mapped);
			//}
            return Ok(lang == "eng" ? qualitys.Select(x => new QualityDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
            }) : qualitys.Select(x => new QualityDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
            }));
        }
		[Authorize(Roles = "Admin")]
		[HttpPut("/Update_Quality")]
        public async Task<ActionResult<bool?>> Update(UpdateQualitiyDto QualityDto)
        {
            var quality = await _unitOfWork.Qualities.GetByIdAsync(e => e.Id == QualityDto.Id);
                
            if (quality == null) return NotFound("Quality Not Found");


            quality.Id = QualityDto.Id;
            quality.Name = QualityDto.Name;
            quality.ArabicName = QualityDto.ArabicName;
			quality.ArabicDescription = QualityDto.ArabicDescription;
          


             _unitOfWork.Qualities.Update(quality);
           
			int res = await _unitOfWork.CompleteAsync();
			if (res > 0)
				return Ok(true);
			return BadRequest("Quality Update operation failed");
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
