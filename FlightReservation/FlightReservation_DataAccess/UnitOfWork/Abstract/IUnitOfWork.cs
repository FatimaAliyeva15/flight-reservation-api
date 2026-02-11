using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_DataAccess.UnitOfWork.Abstract
{
    public interface IUnitOfWork
    {
        public Task<int> SaveAsync();
    }
}
