using AutoMapper;
using FlightReservation_Entities.Concretes;
using FlightReservation_Entities.DTOs.ReservationDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Utilities.Profilies
{
    public class ReservationProfile: Profile
    {
        public ReservationProfile()
        {
            CreateMap<ReservationCreateDto, Reservation>();
            CreateMap<ReservationUpdateDto, Reservation>();
            CreateMap<Reservation, ReservationGetAllDto>();
            CreateMap<Reservation, ReservationGetDto>();
        }
    }
}
