using FlightReservation_Core.Entities.Abstract;
using FlightReservation_Core.Enums.Auth;

namespace FlightReservation_Entities.DTOs.AuthDTOs
{
    public class RegisterDTO: IDto
    {
        public string FullName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public UserRole Role { get; set; }
    }
}
