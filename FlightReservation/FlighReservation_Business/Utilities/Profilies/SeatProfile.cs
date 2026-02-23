using AutoMapper;
using FlightReservation_Entities.Concretes;
using FlightReservation_Entities.DTOs.SeatDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Utilities.Profilies
{
    public class SeatProfile: Profile
    {
        public SeatProfile()
        {
            CreateMap<SeatCreateDto, Seat>();
            CreateMap<SeatUpdateDto, Seat>();
            CreateMap<Seat, SeatGetAllDto>();
            CreateMap<Seat, SeatGetDto>();
        }
    }
}
