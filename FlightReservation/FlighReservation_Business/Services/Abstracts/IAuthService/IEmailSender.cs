using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Services.Abstracts.IAuthService
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
