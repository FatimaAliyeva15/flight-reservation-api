using AutoMapper;
using FlightReservation_Core.Entities.Concrete.Auth;
using FlightReservation_Entities.DTOs.AuthDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Utilities.Profilies
{
    public class AuthProfile: Profile
    {
        public AuthProfile()
        {
            CreateMap<RegisterDTO, AppUser>()
              .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
              .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
              .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));
        }
    }
}
