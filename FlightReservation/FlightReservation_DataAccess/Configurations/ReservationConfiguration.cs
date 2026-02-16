using FlightReservation_Entities.Concretes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_DataAccess.Configurations
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.Property(x => x.TotalPrice).HasColumnType("decimal(18,2)");
            builder.Property(x => x.Status).HasConversion<string>().IsRequired();
            builder.HasOne(x => x.Flight).WithMany(x => x.Reservations).HasForeignKey(x => x.FlightId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.User).WithMany(x => x.Reservations).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(x => x.Tickets).WithOne(x => x.Reservation).HasForeignKey(x => x.ReservationId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Payments).WithOne(x => x.Reservation).HasForeignKey(x => x.ReservationId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
