using FlightReservation_Entities.Concretes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_DataAccess.Configurations
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
            builder.HasOne(x => x.Flight).WithMany(x => x.Tickets).HasForeignKey(x => x.FlightId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Passenger).WithMany(x => x.Tickets).HasForeignKey(x => x.PassengerId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Reservation).WithMany(x => x.Tickets).HasForeignKey(x => x.ReservationId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Seat).WithOne(x => x.Ticket).HasForeignKey<Ticket>(x => x.SeatId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
