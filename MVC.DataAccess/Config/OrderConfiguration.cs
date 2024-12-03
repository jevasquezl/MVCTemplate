using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.DataAccess.Config
{
    internal class OrderConfiguration
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.ApplicationUserId).IsRequired();
            builder.Property(x => x.DateOrder).IsRequired();
            builder.Property(x => x.DeliveryOrder).IsRequired(false);
            builder.Property(x => x.DeliveryNumber).IsRequired(false);
            builder.Property(x => x.Carrier).IsRequired(false);
            builder.Property(x => x.TotalOrder).IsRequired();
            builder.Property(x => x.StateOrder).IsRequired(false);
            builder.Property(x => x.StatePay).IsRequired();
            builder.Property(x => x.DatePay).IsRequired();
            builder.Property(x => x.TransactionId).IsRequired(false);
            builder.Property(x => x.Telephone).IsRequired(false);
            builder.Property(x => x.Address).IsRequired(false);
            builder.Property(x => x.City).IsRequired(false);
            builder.Property(x => x.Country).IsRequired(false);
            builder.Property(x => x.CustomerName).IsRequired();
            builder.Property(x => x.SessionId).IsRequired(false);
            /* Relation */
            builder.HasOne(x => x.ApplicationUser).WithMany()
                .HasForeignKey(x => x.ApplicationUserId)
                .OnDelete(DeleteBehavior.NoAction);

        }

    }
}
