using FlightReservation_Entities.Concretes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_DataAccess.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.Property(x => x.Amount).HasColumnType("decimal(18,2)");
            builder.Property(x => x.Status).HasConversion<string>();
            builder.Property(x => x.PaymentMethod).IsRequired().HasMaxLength(50);
            builder.HasOne(x => x.Reservation).WithMany(x => x.Payments).HasForeignKey(x => x.ReservationId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
