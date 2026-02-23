using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Core.Business.Utilities.Results.Concrete
{
    public class SuccessDataResult<T>: DataResult<T>
    {
        public SuccessDataResult(T data, string message) : base(data, true, message)
        {
        }
        public SuccessDataResult(T data) : base(data, true)
        {
        }
        public SuccessDataResult(string message) : base(default, true, message)
        {
        }
        public SuccessDataResult() : base(default, true)
        {
        }
    }
}
