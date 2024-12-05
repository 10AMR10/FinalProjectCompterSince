
using FinalProject.EF;
using FinalProject.EF.Configuration;
using FinalProject.Core.IRepositories;
using Microsoft.EntityFrameworkCore;
using FinalProject.EF.RepositoriesImplementation;
using FinalProject.Api;
using FinalProject.Core.IRepositories;
using FinalProject.Core;
using Microsoft.AspNetCore.Identity;


namespace FinalProject.Api
{
    public class Program
    {//master
        public static void Main(string[] args)
        {
            //dev
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Database Connection
            //b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)


            //var ConnectionStrings = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection String not found.");
            //builder.Services.AddDbContext<ApplicationDbContext>(options =>
            //        options.UseSqlServer(ConnectionStrings, b => b.MigrationsAssembly("FinalProject.EF")


            //        ));




            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? throw new InvalidOperationException("Connection string not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString, b =>
                    b.MigrationsAssembly("FinalProject.EF") // Explicitly set migrations assembly
                ));


            // Dependency Injection




            //builder.Services.AddScoped<IUnitOfWork, UnitOfWorkImp>();
            //builder.Services.AddScoped<ICollegeRepository, CollegeRepositoryImp>();
            //builder.Services.AddScoped<IEventRepository, EventRepositoryImp>();
            //builder.Services.AddScoped<INewsRepository, NewsRepositoryImp>();
            //builder.Services.AddScoped<IDepartmentRepository, DepartmentRepositoryImp>();
            //builder.Services.AddScoped<IEmployeeRepository, EmployeeRepositoryImp>();
            //builder.Services.AddScoped<ICourseRepository, CourseRepositoryImp>();
            //builder.Services.AddScoped<ICollegeRepository, CollegeRepositoryImp>();
            //builder.Services.AddScoped<IUnitRepository, UnitRepositoryImp>();
            //builder.Services.AddScoped<IAdminRepository, AdminRepositoryImp>();
            //builder.Services.AddScoped<IQualityRepository, QualityRepositoryImp>();



            builder.Services.AddScoped<IUnitOfWork, UnitOfWorkImp>();
            
            builder.Services.AddScoped<IEventRepository, EventRepositoryImp>();
            builder.Services.AddScoped<INewsRepository, NewsRepositoryImp>();
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepositoryImp>();
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepositoryImp>();
            builder.Services.AddScoped<ICourseRepository, CourseRepositoryImp>();
            builder.Services.AddScoped<IUnitRepository, UnitRepositoryImp>();
           
            builder.Services.AddScoped<IQualityRepository, QualityRepositoryImp>();

			builder.Services.AddIdentity<IdentityUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>();
			builder.Services.Configure<IdentityOptions>(options =>
			{
				options.Password.RequireDigit = false;
				options.Password.RequireLowercase = false;
				options.Password.RequireUppercase = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequiredLength = 6;

			});





			//builder.Services.AddScoped<IWebHostEnvironment>();
			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();

            app.UseHttpsRedirection();

			app.UseAuthentication();
			app.UseAuthorization();


			app.MapControllers();

            app.Run();
        }
    }
}
