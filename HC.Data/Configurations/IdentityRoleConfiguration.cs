using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HC.Data.Configurations
{
    class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityRole<int>> builder)
        {
            builder.HasData(
                new IdentityRole<int> {Id = 1, Name = "admin", NormalizedName = "admin".ToUpper()},
                new IdentityRole<int> {Id = 2, Name = "student", NormalizedName = "student".ToUpper()}
            );
        }
    }
}
