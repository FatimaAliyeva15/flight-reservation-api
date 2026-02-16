using FlightReservation_Entities.Concretes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_DataAccess.Configurations
{
    public class FlightConfiguration : IEntityTypeConfiguration<Flight>
    {
        public void Configure(EntityTypeBuilder<Flight> builder)
        {

            builder.Property(f => f.FlightNumber).IsRequired().HasMaxLength(20);
            builder.Property(f => f.Price).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(f => f.AdminComment).HasMaxLength(500);
            builder.Property(f => f.Status).HasConversion<string>();
            builder.HasOne(f => f.Airline).WithMany(f => f.Flights).HasForeignKey(f => f.AirlineId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(f => f.Aircraft).WithMany(f => f.Flights).HasForeignKey(f => f.AircraftId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(f => f.DepartureAirport).WithMany(f => f.DepartureFlights).HasForeignKey(f => f.DepartureAirportId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(f => f.ArrivalAirport).WithMany(f => f.ArrivalFlights).HasForeignKey(f => f.ArrivalAirportId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
