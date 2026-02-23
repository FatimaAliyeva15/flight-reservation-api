using AutoMapper;
using FlightReservation_Entities.Concretes;
using FlightReservation_Entities.DTOs.FlightDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Utilities.Profilies
{
    public class FlightProfile: Profile
    {
        public FlightProfile()
        {
            CreateMap<FlightCreateDto, Flight>();
            CreateMap<FlightUpdateDto, Flight>();
            CreateMap<Flight, FlightGetAllDto>();
            CreateMap<Flight, FlightGetDto>();
        }
    }
}
