using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using HC.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HC.Data.Configurations
{
    class CoursesToStudentsConfiguration : IEntityTypeConfiguration<CourseToStudent>
    {
        public void Configure(EntityTypeBuilder<CourseToStudent> builder)
        {
            builder.HasKey(x => new { x.CourseId, x.StudentId });

            builder.HasOne(x => x.Course)
                .WithMany(x => x.CoursesToStudents)
                .HasForeignKey(x => x.CourseId);

            builder.HasOne(x => x.Student)
                .WithMany(x => x.CoursesToStudents)
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasData(
                new CourseToStudent { CourseId = 1, StudentId = 2, StartDate = DateTime.Now.AddDays(31), EndDate = DateTime.Now.AddDays(44) },
                new CourseToStudent { CourseId = 3, StudentId = 2, StartDate = DateTime.Now.AddDays(5), EndDate = DateTime.Now.AddDays(19) }
            );
        }
    }
}
