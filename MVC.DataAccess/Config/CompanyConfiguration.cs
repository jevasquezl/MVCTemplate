using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.DataAccess.Config
{
    internal class CompanyConfiguration:IEntityTypeConfiguration<Company>
    {

        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Country).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Address).IsRequired();
            builder.Property(x => x.StoreSaleId).IsRequired();
            builder.Property(x => x.CreatedId).IsRequired();
            builder.Property(x => x.UpdatedId).IsRequired();
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.UpdatedDate).IsRequired();

            builder.HasOne(x => x.Store).WithMany()
                .HasForeignKey(x => x.StoreSaleId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Created).WithMany()
                .HasForeignKey(x => x.CreatedId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Updated).WithMany()
                .HasForeignKey(x => x.UpdatedId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}

