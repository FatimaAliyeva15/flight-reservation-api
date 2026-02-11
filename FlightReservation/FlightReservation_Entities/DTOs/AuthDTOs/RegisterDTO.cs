using FlightReservation_Core.Enums.Auth;

namespace FlightReservation_Entities.DTOs.AuthDTOs
{
    public class RegisterDTO
    {
        public string FullName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public UserRole Role { get; set; }
    }
}
