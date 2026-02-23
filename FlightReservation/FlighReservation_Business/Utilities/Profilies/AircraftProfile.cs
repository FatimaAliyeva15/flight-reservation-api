using AutoMapper;
using FlightReservation_Entities.Concretes;
using FlightReservation_Entities.DTOs.AircraftDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Utilities.Profilies
{
    public class AircraftProfile: Profile
    {
        public AircraftProfile()
        {
            CreateMap<AircraftCreateDto, Aircraft>();
            CreateMap<AircraftUpdateDto, Aircraft>();
            CreateMap<Aircraft, AircraftGetAllDto>();
            CreateMap<Aircraft, AircraftGetDto>();
        }
    }
}
