using FinalProject.Core.Models;
using FinalProject.Core.Models.identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace FinalProject.EF.Configuration
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}

		public DbSet<Event> Events { get; set; }
		public DbSet<News> News { get; set; }
		public DbSet<Department> Departments { get; set; }
		public DbSet<Employee> Employees { get; set; }
		public DbSet<Unit> Units { get; set; }
		public DbSet<Course> Courses { get; set; }
		public DbSet<UnitCourses> UnitCourses { get; set; }
		public DbSet<Quality> Qualities { get; set; }
		public DbSet<LevelYear> LevelYears { get; set; }
		public DbSet<Service> Services { get; set; }
		public DbSet<Category> Categories { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Department>()
				.HasMany(d => d.Employees)
				.WithOne(e => e.Department)
				.HasForeignKey(e => e.DepartmentId)
				.OnDelete(DeleteBehavior.SetNull);



			modelBuilder.Entity<Department>()
				.HasMany(d => d.Courses)
				.WithOne(e => e.Department)
				.HasForeignKey(f => f.DepartmentId)
				.OnDelete(DeleteBehavior.SetNull);



			modelBuilder.Entity<ApplicationUser>()
				.HasOne(u => u.employee)
				.WithOne(e => e.applicationUser)
				.HasForeignKey<ApplicationUser>(e => e.EmployeId)
				.OnDelete(DeleteBehavior.SetNull);


			modelBuilder.Entity<UnitCourses>()
				.HasOne(x => x.unit)
				.WithMany(u => u.unitCourses)
				.HasForeignKey(f => f.UnitId)
				.OnDelete(DeleteBehavior.SetNull);

			modelBuilder.Entity<Unit>()
				.HasMany(u => u.UnitEmployees)
				.WithOne(e => e.Unit)
				.HasForeignKey(e => e.UnitId)
				.OnDelete(DeleteBehavior.SetNull);

			modelBuilder.Entity<Employee>()
				.HasOne(e => e.Department)
				.WithMany(d => d.Employees)
				.HasForeignKey(e => e.DepartmentId)
				.OnDelete(DeleteBehavior.SetNull);
			
			modelBuilder.Entity<Service>()
				.HasOne(s=> s.category)
				.WithMany(c=>c.Services)
				.HasForeignKey(s=> s.categoryId)
				.OnDelete(DeleteBehavior.SetNull);
			modelBuilder.Entity<Course>()
				.HasOne(s => s.levelYear)
				.WithMany(c => c.Courses)
				.HasForeignKey(s => s.LevelYearId)
				.OnDelete(DeleteBehavior.SetNull);

		}


	}
}
