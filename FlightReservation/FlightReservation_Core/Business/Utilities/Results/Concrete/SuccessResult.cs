using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Core.Business.Utilities.Results.Concrete
{
    public class SuccessResult: Result
    {
        public SuccessResult(string message) : base(true, message)
        {
        }
        public SuccessResult() : base(true)
        {
        }
    }
}
