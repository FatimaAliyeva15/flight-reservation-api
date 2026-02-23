using FlightReservation_Core.Business.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Core.Business.Utilities.Results.Concrete
{
    public class DataResult<T>: Result, IDataResult<T>
    {
        public DataResult(T data, bool success, string message) : base(success, message)
        {
            Data = data;
        }
        public DataResult(T data, bool success) : base(success)
        {
            Data = data;
        }
        public T Data { get; }
    }
}
