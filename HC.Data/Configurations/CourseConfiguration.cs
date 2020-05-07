using System;
using System.Collections.Generic;
using System.Text;
using HC.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HC.Data.Configurations
{
    class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasOne(x => x.Admin)
                .WithMany(x => x.Courses)
                .HasForeignKey(x => x.CreatorId);

            builder.HasMany(x => x.CoursesToStudents)
                .WithOne(x => x.Course);
        }
    }
}
