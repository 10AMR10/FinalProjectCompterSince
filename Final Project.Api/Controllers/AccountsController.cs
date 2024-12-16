using DiabetesApp.API.Dtos;
using DiabetesApp.Core.Service.Contract;
using FinalProject.Core.Models.identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;
using System.Security.Claims;
using Talabat.APIs.Errors;

namespace DiabetesApp.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountsController : ControllerBase
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		private readonly ITokentService _tokentService;


		public AccountsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager
			, ITokentService tokentService)
		{
			this._userManager = userManager;
			this._signInManager = signInManager;

			this._tokentService = tokentService;

		}
		// Make Login (Login Dto)
		[HttpPost("Login/{lang}")]
		public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto input,string lang)
		{
			var user = await _userManager.FindByEmailAsync(input.Email);
			
			if (user is null)
			{
				if (lang == "eng") 
					return BadRequest(new ApiResponse(400, "Email Not Exist"));
				else
					return BadRequest(new ApiResponse(400, "الايميل ليس موجود"));

			}
			var res = await _signInManager.CheckPasswordSignInAsync(user, input.Password, false);
			if (!res.Succeeded)
			{
				if (lang == "eng")
					return BadRequest(new ApiResponse(400, "Wrong Password"));
				else
					return BadRequest(new ApiResponse(400, "كلمه المرور غير صحيحه"));
			}


			return Ok(new UserDto
			{
				Id = user.Id,
				Email = input.Email,

				UserName = user.UserName,

				Token = await _tokentService.CreateTokenAsync(user, _userManager),
				Role = string.Join("", await _userManager.GetRolesAsync(user))
			});
		}
		// create user  (RegisterDto) => UserDto
		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<ActionResult<UserDto>> CreateUser(RegisterDto input)
		{
			if (await _userManager.FindByEmailAsync(input.email) is not null)
				return BadRequest(new ApiResponse(400, "Dublicated Email"));
			var user = new ApplicationUser
			{
				Email = input.email,
				
				UserName = input.userName,
			};
			var res = await _userManager.CreateAsync(user, input.password);
			if (!res.Succeeded)
			{
				// Collect detailed error messages
				var errorDetails = res.Errors
					.Select(e => $"Code: {e.Code}, Description: {e.Description}")
					.ToList();

				// Return a detailed BadRequest response
				return BadRequest(new ApiResponse(400, string.Join(" | ", errorDetails)));
			}

			await _userManager.AddToRoleAsync(user, "Doctor");
			return Ok(new UserDto
			{

				Role = string.Join("", await _userManager.GetRolesAsync(user)),
				UserName = user.UserName,

				Token = await _tokentService.CreateTokenAsync(user, _userManager)
			});
		}
		// get all users () => list of usersDto
		[Authorize(Roles = "Admin")]
		[HttpGet("AllUsers")]
		public async Task<ActionResult<IReadOnlyList<UserDto>>> GetAllUsers()
		{
			var users = await _userManager.Users.ToListAsync();
			List<ApplicationUser> EmpUsers = new List<ApplicationUser>();
			foreach (var user in users)
			{
				var role = await _userManager.GetRolesAsync(user);
				if (role.Contains("Employee"))
					EmpUsers.Add(user);
			}
			//var hospitals= await _unitOfWork.GetRepo<Hospitail>().GetAllAsync();
			//var hosIds
			if (EmpUsers.Count == 0)
				return NotFound(new ApiResponse(404, "Users Not Found"));
			var mapped = EmpUsers.Select(x => new UserToReturnDto
			{
				Id = x.Id,
				Email = x.Email,
				UserName = x.UserName,

			}
				);
			var userDto = mapped.Select(x => new UserToReturnDto
			{
				Id = x.Id,
				Email = x.Email,
				UserName = x.UserName,
				Role = "Employee"
			});
			return Ok(userDto);
		}
		[Authorize(Roles = "Admin")]
		[HttpDelete("Delete/{email}")]
		public async Task<ActionResult<bool>> DeleteUser(string email)
		{
			var user = await _userManager.FindByEmailAsync(email);
			if (user is null)
				return BadRequest(new ApiResponse(400));
			var res = await _userManager.DeleteAsync(user);
			if (!res.Succeeded)
			{
				// Collect detailed error messages
				var errorDetails = res.Errors
					.Select(e => $"Code: {e.Code}, Description: {e.Description}")
					.ToList();

				// Return a detailed BadRequest response
				return BadRequest(new ApiResponse(400, string.Join(" | ", errorDetails)));
			}
			return Ok(true);
		}
		
		[HttpGet("CurrentUser")]
		public async Task<ActionResult<UserDto>> GetCurrentUser()
		{
			var email = User.FindFirstValue(ClaimTypes.Email);
			var user = await _userManager.FindByEmailAsync(email);
			
			return Ok(new UserToReturnDto
			{
				Id = user.Id,
				Email = user.Email,
				UserName = user.UserName,
				
			});
		}


	}
}
