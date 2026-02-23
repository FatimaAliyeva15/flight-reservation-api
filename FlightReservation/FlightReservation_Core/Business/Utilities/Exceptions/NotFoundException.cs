using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Core.Business.Utilities.Exceptions
{
    public class NotFoundException: Exception
    {
        public NotFoundException(string? message) : base(message)
        {

        }

        public NotFoundException()
        {

        }

    }
}
