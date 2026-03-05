using FlightReservation_Core.Business.Utilities.Results.Abstract;
using FlightReservation_Entities.DTOs.AuthDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Services.Abstracts.IAuthService
{
    public interface IAuthService
    {
        public Task<IResult> Register(RegisterDTO register);
        public Task<IDataResult<AuthResponseDto>> Login(LoginDto login);
        public Task<IResult> AddAdmin(RegisterDTO register);
        Task<IResult> ConfirmEmail(string userId, string token);    
        Task<IResult> ForgotPassword(string email);           
        Task<IResult> ResetPassword(ResetPasswordDTO reset);
    }
}
