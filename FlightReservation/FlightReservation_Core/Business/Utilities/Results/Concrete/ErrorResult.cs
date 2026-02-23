using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Core.Business.Utilities.Results.Concrete
{
    public class ErrorResult:Result
    {
        public ErrorResult(string message) : base(false, message)
        {
        }
        public ErrorResult() : base(false)
        {
        }
    }
}
