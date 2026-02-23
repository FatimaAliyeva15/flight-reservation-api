using AutoMapper;
using FlightReservation_Entities.Concretes;
using FlightReservation_Entities.DTOs.PassengerDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Utilities.Profilies
{
    public class PassengerProfile: Profile
    {
        public PassengerProfile()
        {
            CreateMap<PassengerCreateDto, Passenger>();
            CreateMap<PassengerUpdateDto, Passenger>();
            CreateMap<Passenger, PassengerGetAllDto>();
            CreateMap<Passenger, PassengerGetDto>();
        }
    }
}
