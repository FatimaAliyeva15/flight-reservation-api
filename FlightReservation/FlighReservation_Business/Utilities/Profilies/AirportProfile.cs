using AutoMapper;
using FlightReservation_Entities.Concretes;
using FlightReservation_Entities.DTOs.AirportDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Utilities.Profilies
{
    public class AirportProfile: Profile
    {
        public AirportProfile()
        {
            CreateMap<AirportCreateDto, Airport>();
            CreateMap<AirportUpdateDto, Airport>();
            CreateMap<Airport, AirportGetAllDto>();
            CreateMap<Airport, AirportGetDto>();
        }
    }
}
