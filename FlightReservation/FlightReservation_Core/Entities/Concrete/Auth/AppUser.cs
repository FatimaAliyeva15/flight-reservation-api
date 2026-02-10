using FlightReservation_Entities.Concretes;
using FlightReservation_Entities.Enums.Auth;
using Microsoft.AspNetCore.Identity;


namespace FlightReservation_Core.Entities.Concrete.Auth
{
    public class AppUser: IdentityUser
    {
        public string FullName { get; set; }

        public bool IsActive { get; set; } = true;

        public UserRole Role { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
    }
}
