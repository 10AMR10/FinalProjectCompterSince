
using FinalProject.Core;
using FinalProject.Core.IRepositories;
using FinalProject.Core.Models.identity;
using FinalProject.EF;
using FinalProject.EF.Configuration;
using FinalProject.EF.Identity;
using FinalProject.EF.RepositoriesImplementation;
using FinalProject.EF.Token;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace FinalProject.Api
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowAll", builder =>
				{
					builder.AllowAnyOrigin()
						   .AllowAnyMethod()
						   .AllowAnyHeader();
				});
			});

			builder.Services.AddControllers();

			// Swagger configuration
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			// Database Configuration
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
			);

			// Dependency Injection
			builder.Services.AddScoped<IUnitOfWork, UnitOfWorkImp>();
			builder.Services.AddScoped<IEventRepository, EventRepositoryImp>();
			builder.Services.AddScoped<INewsRepository, NewsRepositoryImp>();
			builder.Services.AddScoped<IDepartmentRepository, DepartmentRepositoryImp>();
			builder.Services.AddScoped<IEmployeeRepository, EmployeeRepositoryImp>();
			builder.Services.AddScoped<ICourseRepository, CourseRepositoryImp>();
			builder.Services.AddScoped<IUnitRepository, UnitRepositoryImp>();
			builder.Services.AddScoped<IQualityRepository, QualityRepositoryImp>();

			// Identity Configuration
			builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>();
			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
						ValidateAudience = true,
						ValidAudience = builder.Configuration["JWT:ValidAudience"],
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
						ClockSkew = TimeSpan.Zero
					};
				});
			builder.Services.Configure<IdentityOptions>(options =>
			{
				options.Password.RequireDigit = false;
				options.Password.RequireLowercase = false;
				options.Password.RequireUppercase = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequiredLength = 6;
			});

			builder.Services.AddScoped<ITokenService, TokenService>();

			var app = builder.Build();

			// Seeding Identity Data
			using (var scope = app.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				var loggerFactory = services.GetRequiredService<ILoggerFactory>();

				try
				{
					var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
					var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
					await AppIdentityDbContextSeeding.SeedingIdentityAsync(userManager, roleManager);
				}
				catch (Exception ex)
				{
					var logger = loggerFactory.CreateLogger<Program>();
					logger.LogError(ex, "An error occurred while seeding the database.");
				}
			}

			// Middleware configuration
			
				app.UseSwagger();
				app.UseSwaggerUI();
			

			app.UseStaticFiles();
			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseCors("AllowAll");

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
