using AutoMapper;
using FlightReservation_Entities.Concretes;
using FlightReservation_Entities.DTOs.AirlineDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Utilities.Profilies
{
    public class AirlineProfile: Profile
    {
        public AirlineProfile()
        {
            CreateMap<AirlineCreateDto, Airline>();
            CreateMap<AirlineUpdateDto, Airline>();
            CreateMap<Airline, AirlineGetAllDto>();
            CreateMap<Airline, AirlineGetDto>();
        }
    }
}
