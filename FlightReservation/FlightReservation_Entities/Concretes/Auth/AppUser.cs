using FlightReservation_Core.Enums.Auth;
using FlightReservation_Entities.Concretes;
using Microsoft.AspNetCore.Identity;


namespace FlightReservation_Core.Entities.Concrete.Auth
{
    public class AppUser: IdentityUser
    {
        public string FullName { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}
