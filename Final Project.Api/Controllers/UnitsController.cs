using FinalProject.Core.Dtos.DepartmentDtos;
using FinalProject.Core.Dtos.UnitDots;
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
    public class UnitController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnitController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // POST: api/Unit
        [HttpPost]
        public async Task<IActionResult> Create( UnitCreateDto unitDto)
        {
            if (unitDto == null)
                return BadRequest("Invalid unit data.");

            //var headOfUnit =await _unitOfWork.Employees.GetByIdAsync(e => e.EmployeeId == unitDto.Head_Of_UnitId);
            //if (headOfUnit == null)
            //    return BadRequest("Head of unit not found.");

            //var employees =await _unitOfWork.Employees.GetAllAsync(e => unitDto.EmployeeIds.Contains(e.EmployeeId));
            //if (employees == null )
            //    return BadRequest("Employees not found.");

            var unit = new Unit()
            {
                Name = unitDto.Name,
                Description = unitDto.Description,
                //Head_Of_Unit = headOfUnit,
                //Employees = employees.ToList() 
            };

            var createdUnit = await _unitOfWork.Units.AddAsync(unit);
            if (createdUnit == null)
                return BadRequest("Failed to create unit.");

            await _unitOfWork.CompleteAsync();

            return Ok(unit);
            //return CreatedAtAction(nameof(GetById), new { id = createdUnit.UnitId }, createdUnit);
        }

        // GET: api/Unit/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var unit =await _unitOfWork.Units.GetByIdAsync(u=> u.UnitId == id, new[] { "Head_Of_Unit", "Employees"  });
            if (unit == null)
                return NotFound("Unit not Found");

            return Ok(unit);
        }

        // GET: api/Unit
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var units =await _unitOfWork.Units.GetAllAsync( null, new[] { "Head_Of_Unit", "Employees" });
            if (units == null || !units.Any())
                return NotFound("There is no unit created yet.");

            return Ok(units);
        }

        // PUT: api/Unit/
        [HttpPut("")]
        public async Task<IActionResult> Update( [FromBody] UnitUpdateDto unitDto)
        {
            if (unitDto == null)
                return BadRequest("Invalid unit data.");

            var headOfUnit = await _unitOfWork.Employees.GetByIdAsync(e => e.EmployeeId == unitDto.Head_Of_UnitId);
            if (headOfUnit == null)
                return BadRequest("Head of unit not found.");



            //var employees = await _unitOfWork.Employees.GetAllAsync(e => unitDto.EmployeeIds.Contains(e.EmployeeId));
            //if (employees == null)
            //    return BadRequest("Employees not found.");



            var unit =await _unitOfWork.Units.GetByIdAsync(u => u.UnitId == unitDto.UnitId);
            if (unit == null)
                return NotFound("Unit not found");

             unit = new Unit()
            {
                 UnitId = unitDto.UnitId,
                Name = unitDto.Name,
                Description = unitDto.Description,
                Head_Of_Unit = headOfUnit,
                //Employees = employees.ToList()
            };

            //unit.Name = unitDto.Name;
            //unit.Description = unitDto.Description;
            //unit.Head_Of_Unit = _unitOfWork.Employee.
            //    (e => e.EmployeeId == unitDto.HeadOfUnitId);
            //unit.Employees = _unitOfWork.Employee.GetAllAsync(e => unitDto.EmployeeIds.Contains(e.EmployeeId)).ToList();

            var UnitUpdated = await _unitOfWork.Units.UpdateAsync(unit);
            if (UnitUpdated == null) return BadRequest("Unit Update operation failed");
            await _unitOfWork.CompleteAsync();

            return Ok(unit); 
        }


        [HttpPut("/Add_Emloyee_To_Unit")]
        public async Task<IActionResult> AddEmployeeToUnit(AddEmployeeToUnit AddEmployeeDto)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(e => e.EmployeeId == AddEmployeeDto.EmployeeId);
            if (employee == null) return NotFound("Employee not found");

            var unit = await _unitOfWork.Units.GetByIdAsync(u => u.UnitId == AddEmployeeDto.UnitId);
            if (unit == null) return NotFound("Unit not found");

            var employeeAdded = await _unitOfWork.Units.AddEmployeeAsync(AddEmployeeDto.UnitId, AddEmployeeDto.EmployeeId);

            if (employeeAdded == null) return BadRequest("Add employee operation failed");
           
            await _unitOfWork.CompleteAsync();
            return Ok(employeeAdded);

        }

        [HttpPut("/Remove_Emloyee_From_Unit")]
        public async Task<IActionResult> RemoveEmployeeFromUnit(AddEmployeeToUnit AddEmployeeDto)
        {

            var employee = await _unitOfWork.Employees.GetByIdAsync(e => e.EmployeeId == AddEmployeeDto.EmployeeId);
            if (employee == null) return NotFound("Employee not found");

            var unit = await _unitOfWork.Units.GetByIdAsync(u => u.UnitId == AddEmployeeDto.UnitId);
            if (unit == null) return NotFound("Unit not found");

            var employeeRemoved = await _unitOfWork.Units.RemoveEmployeeAsync(AddEmployeeDto.UnitId, AddEmployeeDto.EmployeeId);

            if (employeeRemoved == null) return BadRequest("Remove employee operation failed");
            
            await _unitOfWork.CompleteAsync();
            return Ok(employeeRemoved);

        }

        // DELETE: api/Unit/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var unit =await _unitOfWork.Units.GetByIdAsync(u=> u.UnitId ==id, new[] { "Head_Of_Unit", "Employees" });
            if (unit == null)
                return NotFound("Unit not found.");

            var UnitDeleted = await _unitOfWork.Units.DeleteAsync(id);
            if (UnitDeleted == null) return BadRequest("Unit delete operation failed");

            await _unitOfWork.CompleteAsync();
            return NoContent();
        }
    }
}
