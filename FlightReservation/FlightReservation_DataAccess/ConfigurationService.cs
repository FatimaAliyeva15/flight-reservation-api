using FlightReservation_DataAccess.EFCore;
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
        extension(IServiceCollection services)
        {
            public IServiceCollection AddDataAccessConfiguration(IConfiguration configuration)
            {
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("Default"));
                });

                //services.AddScoped<IUnitOfWork, UnitOfWork.Concrete.UnitOfWork>();

                return services;
            }
        }
    }
}
