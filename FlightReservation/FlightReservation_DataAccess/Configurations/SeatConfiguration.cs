using FlightReservation_Entities.Concretes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_DataAccess.Configurations
{
    public class SeatConfiguration : IEntityTypeConfiguration<Seat>
    {
        public void Configure(EntityTypeBuilder<Seat> builder)
        {
            builder.Property(x => x.SeatNumber).IsRequired().HasMaxLength(10);
            builder.Property(x => x.Class).IsRequired().HasMaxLength(20);
            builder.HasOne(x => x.Flight).WithMany(x => x.Seats).HasForeignKey(x => x.FlightId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Ticket).WithOne(x => x.Seat).HasForeignKey<Seat>(x => x.TicketId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
