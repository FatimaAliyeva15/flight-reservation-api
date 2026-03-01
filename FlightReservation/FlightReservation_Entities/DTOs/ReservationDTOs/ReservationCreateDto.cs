using FlightReservation_Core.Entities.Abstract;


namespace FlightReservation_Entities.DTOs.ReservationDTOs
{
    public class ReservationCreateDto: IDto
    {
        public Guid FlightId { get; set; }
        public List<Guid> SeatIds { get; set; }
    }
}

