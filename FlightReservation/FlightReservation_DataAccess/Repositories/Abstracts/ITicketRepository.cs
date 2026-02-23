using FlightReservation_Core.DataAccess.Abstract;
using FlightReservation_Entities.Concretes;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_DataAccess.Repositories.Abstracts
{
    public interface ITicketRepository: IBaseRepository<Ticket>
    {
    }
}
