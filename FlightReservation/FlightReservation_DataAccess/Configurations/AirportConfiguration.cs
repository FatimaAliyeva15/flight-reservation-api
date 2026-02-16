using FlightReservation_Entities.Concretes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_DataAccess.Configurations
{
    public class AirportConfiguration : IEntityTypeConfiguration<Airport>
    {
        public void Configure(EntityTypeBuilder<Airport> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.City).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Country).IsRequired().HasMaxLength(100);
            builder.Property(x => x.IATACode).IsRequired().HasMaxLength(10);
            builder.HasIndex(x => x.IATACode).IsUnique();
        }
    }
}
