using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HC.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HC.Data
{
    public interface IHCDbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseToStudent> CoursesToStudents { get; set; }
        Task<int> SaveChangesAsync();
    }
}
