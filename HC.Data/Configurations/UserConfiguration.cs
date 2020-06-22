using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;
using HC.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HC.Data.Configurations
{
    class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasMany(x => x.Courses)
                .WithOne(x => x.Creator);

            builder.HasMany(x => x.CoursesToStudents)
                .WithOne(x => x.Student);

            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(64);

            builder.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(64);

            builder.Property(x => x.Email)
                .IsRequired();

            builder.Property(x => x.DateOfBirth)
                .IsRequired();

            builder.HasData(
                new User
                {
                    Id = 1,
                    UserName = "Bodkia",
                    FirstName = "Bohdan",
                    LastName = "Borodulin",
                    Email = "bodkia2000@gmail.com",
                    PhoneNumber = "0685701034",
                    PasswordHash = "qwerty",
                    DateOfBirth = new DateTime(2000, 4, 8),
                    RegisteredDate = DateTime.Now
                },
                new User
                {
                    Id = 2,
                    UserName = "DariyaKleer",
                    FirstName = "Dariya",
                    LastName = "Misko",
                    Email = "dk@gmail.com",
                    PhoneNumber = "0974570655",
                    PasswordHash = "qwerty",
                    DateOfBirth = new DateTime(2002, 7, 15),
                    RegisteredDate = DateTime.Now
                });
        }
    }
}
