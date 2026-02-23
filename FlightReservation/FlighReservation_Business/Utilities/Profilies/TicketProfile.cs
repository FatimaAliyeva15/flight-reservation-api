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
            CreateMap<Ticket, TicketGetAllDto>();
            CreateMap<Ticket, TicketGetDto>();
        }
    }
}
