using FlightReservation_Entities.Concretes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_DataAccess.Configurations
{
    public class AirlineConfiguration : IEntityTypeConfiguration<Airline>
    {
        public void Configure(EntityTypeBuilder<Airline> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Code).IsRequired().HasMaxLength(100);

            builder.HasMany(x => x.Aircrafts).WithOne(x => x.Airline).HasForeignKey(x => x.AirlineId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(x => x.Flights).WithOne(x => x.Airline).HasForeignKey(x => x.AirlineId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
