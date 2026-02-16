using FlightReservation_Core.Entities.Abstract;


namespace FlightReservation_Entities.DTOs.ReservationDTOs
{
    public class ReservationCreateDto: IDto
    {
        public Guid FlightId { get; set; }
        public Guid UserId { get; set; }
        public List<Guid> PassengerIds { get; set; }
    }
}

