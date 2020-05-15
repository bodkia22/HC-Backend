using System;
using System.Threading;
using System.Threading.Tasks;
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
        public DbSet<CourseToStudent> CoursesToStudents { get; set; }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(HCDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

    }
}
