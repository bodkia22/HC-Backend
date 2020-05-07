using System;
using System.Collections.Generic;
using System.Text;
using HC.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HC.Data.Configurations
{
    class AdminConfiguration : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.HasMany(x => x.Courses)
                .WithOne(x => x.Admin);

            builder.HasOne(x=> x.User)
                .WithOne(x=>x.Admin)
                .HasForeignKey<Admin>(x=>x.Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
