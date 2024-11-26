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
    internal class KardexConfiguration : IEntityTypeConfiguration<Kardex>
    {
        public void Configure(EntityTypeBuilder<Kardex> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.StoreProductId).IsRequired();
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.Detail).IsRequired();
            builder.Property(x => x.LastStock).IsRequired();
            builder.Property(x => x.Stock).IsRequired();
            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x => x.CostUnit).IsRequired();
            builder.Property(x => x.Total).IsRequired();
            builder.Property(x => x.ApplicationUserId).IsRequired();
            builder.Property(x => x.RegisterDate).IsRequired();

            /* Relation */
            builder.HasOne(x => x.StoreProduct).WithMany()
                .HasForeignKey(x => x.StoreProductId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.ApplicationUser).WithMany()
           .HasForeignKey(x => x.ApplicationUserId)
           .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
