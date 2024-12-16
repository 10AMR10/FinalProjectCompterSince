using FinalProject.Core.Models;
using FinalProject.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProject;
using FinalProject.EF;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using FinalProject.Core.Models.identity;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        public DbSet<Course> Courses{ get; set; }
        public DbSet<UnitCourses> UnitCourses { get; set; }
        public DbSet<Quality> Qualities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

          


			modelBuilder.Entity<Department>()
                .HasMany(d => d.Employees)
                .WithOne(e => e.Department)
                .HasForeignKey(e=> e.DepartmentId)
                .OnDelete(DeleteBehavior.NoAction);



            modelBuilder.Entity<Department>()
                .HasMany(d => d.Courses)
                .WithOne(e => e.Department)
                .HasForeignKey(f=> f.DepartmentId);


           
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.employee)
                .WithOne(e => e.applicationUser)
                .HasForeignKey<ApplicationUser>(e => e.EmployeId);


			modelBuilder.Entity<UnitCourses>()
                .HasOne(x=> x.unit)
                .WithMany(u=>u.unitCourses)
                .HasForeignKey(f=>f.UnitId);

            modelBuilder.Entity<Unit>()
                .HasMany(u => u.UnitEmployees)
                .WithOne(e => e.Unit)
                .HasForeignKey(e=> e.UnitId);


		}


    }
}
