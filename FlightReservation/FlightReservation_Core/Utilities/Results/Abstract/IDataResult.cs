using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Core.Utilities.Results.Abstract
{
    public interface IDataResult<T>
    {
        public T Data { get; }
        public bool Success { get; }
        public string Message { get; }
    }
}
