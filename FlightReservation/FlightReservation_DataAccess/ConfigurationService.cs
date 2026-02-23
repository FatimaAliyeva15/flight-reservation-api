using FlightReservation_DataAccess.EFCore;
using FlightReservation_DataAccess.UnitOfWork.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_DataAccess
{
    public static class ConfigurationService
    {
        public static IServiceCollection AddDataAccessConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("Default"));
            });
            services.AddScoped<IUnitOfWork, UnitOfWork.Concrete.UnitOfWork>();
            return services;
        }
    }
 }
