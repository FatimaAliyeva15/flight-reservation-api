using FlightReservation_Core.DataAccess.Concrete;
using FlightReservation_DataAccess.EFCore;
using FlightReservation_DataAccess.Repositories.Abstracts;
using FlightReservation_Entities.Concretes;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_DataAccess.Repositories.Concretes
{
    public class PaymentRepository : BaseRepository<Payment, AppDbContext>, IPaymentRepository
    {
        public PaymentRepository(AppDbContext context) : base(context)
        {
        }
    }
}
