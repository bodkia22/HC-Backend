using System;
using System.Collections.Generic;
using System.Text;
using HC.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HC.Data
{
    public interface IHCDbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseToStudent> CoursesToUsers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<User> AppUsers { get; set; }
    }
}
