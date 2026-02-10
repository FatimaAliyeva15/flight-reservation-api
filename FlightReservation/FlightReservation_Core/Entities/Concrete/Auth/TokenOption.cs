using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Core.Entities.Concrete.Auth
{
    public class TokenOption
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int AccessTokenExpiration { get; set; }
        public string SecurityKey { get; set; }
    }
}
