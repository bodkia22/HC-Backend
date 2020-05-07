using System;
using HC.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace HC.Data
{
    public class HCDbContext : IdentityDbContext<User,IdentityRole<int>,int>, IHCDbContext
    {
        public HCDbContext(DbContextOptions<HCDbContext> options) : base(options) { }

        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseToStudent> CoursesToUsers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<User> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(HCDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
