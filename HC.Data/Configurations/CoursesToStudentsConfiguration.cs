using System;
using System.Collections.Generic;
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
                .HasForeignKey(x => x.CourseId)
                .IsRequired();

            builder.HasOne(x => x.Student)
                .WithMany(x => x.CoursesToStudents)
                .HasForeignKey(x => x.StudentId)
                .IsRequired();
        }
    }
}
