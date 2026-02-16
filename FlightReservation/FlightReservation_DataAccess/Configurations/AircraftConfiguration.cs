using FlightReservation_Entities.Concretes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_DataAccess.Configurations
{
    public class AircraftConfiguration : IEntityTypeConfiguration<Aircraft>
    {
        public void Configure(EntityTypeBuilder<Aircraft> builder)
        {
            builder.Property(x => x.Model).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Capacity).IsRequired().HasMaxLength(100);
            builder.HasOne(f => f.Airline).WithMany(a => a.Aircrafts).HasForeignKey(f => f.AirlineId).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
