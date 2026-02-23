using AutoMapper;
using FlightReservation_Entities.Concretes;
using FlightReservation_Entities.DTOs.PaymentDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Utilities.Profilies
{
    public class PaymentProfile: Profile
    {
        public PaymentProfile()
        {
            CreateMap<PaymentCreateDto, Payment>();
            CreateMap<PaymentUpdateDto, Payment>();
            CreateMap<Payment, PaymentGetAllDto>();
            CreateMap<Payment, PaymentGetDto>();
        }
    }
}
