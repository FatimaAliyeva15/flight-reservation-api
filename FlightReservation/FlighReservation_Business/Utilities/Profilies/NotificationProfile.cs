using AutoMapper;
using FlightReservation_Entities.Concretes;
using FlightReservation_Entities.DTOs.NotificationDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Utilities.Profilies
{
    public class NotificationProfile: Profile
    {
        public NotificationProfile()
        {
            CreateMap<NotificationCreateDto, Notification>();
            CreateMap<NotificationUpdateDto, Notification>();
            CreateMap<Notification, NotificationGetAllDto>().ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.AppUser.UserName));
            CreateMap<Notification, NotificationGetDto>().ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.AppUser.UserName));
        }
    }
}
