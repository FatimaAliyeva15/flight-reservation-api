using FlightReservation_Entities.Concretes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_DataAccess.Configurations
{
    public class PassengerConfiguration : IEntityTypeConfiguration<Passenger>
    {
        public void Configure(EntityTypeBuilder<Passenger> builder)
        {
            builder.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(p => p.LastName).IsRequired().HasMaxLength(100);
            builder.Property(p => p.PassportNumber).IsRequired().HasMaxLength(50);
            builder.HasIndex(p => p.PassportNumber).IsUnique();
            builder.HasMany(p => p.Tickets).WithOne(p => p.Passenger).HasForeignKey(p => p.PassengerId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
