using FlighReservation_Business.Services.Abstracts;
using FlighReservation_Business.Services.Concretes;
using FlighReservation_Business.Utilities.Profilies;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace FlighReservation_Business
{
    public static class ConfigurationService
    {
        public static IServiceCollection AddBusinessConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddAutoMapper(typeof(AircraftProfile));
            services.AddFluentValidationAutoValidation()
                    .AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddScoped<IAircraftService, AircraftService>();
            services.AddScoped<IAirlineService, AirlineService>();
            services.AddScoped<IAirportService, AirportService>();
            services.AddScoped<IPassengerService, PassengerService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IFlightService, FlightService>();
            services.AddScoped<ISeatService, SeatService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<IPaymentService, PaymentService>();



            return services;
        }
    }
}
