using AutoMapper;
using FlightReservation_Entities.Concretes;
using FlightReservation_Entities.DTOs.TicketDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Utilities.Profilies
{
    public class TicketProfile: Profile
    {
        public TicketProfile()
        {
            CreateMap<TicketCreateDto, Ticket>();
            CreateMap<TicketUpdateDto, Ticket>();
            CreateMap<Ticket, TicketGetAllDto>().ForMember(dest => dest.PassengerName,
               opt => opt.MapFrom(src => src.Passenger.FirstName)).ForMember(dest => dest.SeatNumber,
               opt => opt.MapFrom(src => src.Seat.SeatNumber)); ;
            CreateMap<Ticket, TicketGetDto>();
        }
    }
}
