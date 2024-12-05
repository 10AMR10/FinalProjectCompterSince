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

        [HttpPost("/Add_Quality")]
        public async Task<IActionResult> Create(AddQualityDto QualityDto)
        {
            var Quality = new Core.Models.Quality()
            {
                Name = QualityDto.Name,
                Description = QualityDto.Description,
            };
            var QualityAdded = await _unitOfWork.Qualities.AddAsync(Quality);
            if (QualityAdded == null)
                return BadRequest("Bad Request");


            await _unitOfWork.CompleteAsync();
            return Ok(Quality);
        }

        [HttpGet("/Get_Quality_By_Id/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var Quality = await _unitOfWork.Qualities.GetByIdAsync(e => e.Id == id);
            if (Quality == null) return NotFound("News not found");

            return Ok(Quality);
        }

        [HttpGet("/Get_All_Qualitys")]
        public async Task<IActionResult> GetAll()
        {
            var Qualitys = await _unitOfWork.Qualities.GetAllAsync(null);
            if (Qualitys == null) return NotFound("There is no qualities created");

            return Ok(Qualitys);
        }

        [HttpPut("/Update_Quality")]
        public async Task<IActionResult> Update(QualityDto QualityDto)
        {
            var Quality = await _unitOfWork.Qualities.GetByIdAsync(e => e.Id == QualityDto.Id);

            if (Quality == null) return NotFound("Quality Not Found");

            Quality = new()
            {
                Id = QualityDto.Id,
                Name = QualityDto.Name,
                Description = QualityDto.Description,
            };


            var QualityUpdated = await _unitOfWork.Qualities.UpdateAsync(Quality);
            if (QualityUpdated == null) return BadRequest("Quality Update operation failed");

            await _unitOfWork.CompleteAsync();
            return Ok(Quality);
        }

        [HttpDelete("/Delete_Quality/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var Quality = await _unitOfWork.Qualities.GetByIdAsync(e => e.Id == id);
            if (Quality == null)
                return NotFound("Quality Not Found");

            var QualityDeleted = await _unitOfWork.Qualities.DeleteAsync(id);
            if (QualityDeleted == null) return BadRequest("Quality delete operation failed");

            await _unitOfWork.CompleteAsync();
            return Ok(Quality);
        }
    }
}
