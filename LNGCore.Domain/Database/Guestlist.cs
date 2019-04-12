using System;
using System.Collections.Generic;

namespace LNGCore.Domain.Database
{
    public partial class Guestlist
    {
        public int GuestId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Guests { get; set; }
        public string Children { get; set; }
        public string Street { get; set; }
        public string Zip { get; set; }
        public string State { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Notes { get; set; }
        public string TotalGuests { get; set; }
        public string City { get; set; }
        public string WhoIsWho { get; set; }
    }
}
