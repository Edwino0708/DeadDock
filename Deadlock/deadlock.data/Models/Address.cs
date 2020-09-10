using System;
using System.Collections.Generic;

namespace deadlock.data.Models
{
    public partial class Address
    {
        public Guid Id { get; set; }
        public Guid CityId { get; set; }
        public string StreetName { get; set; }
        public decimal? HouseNumber { get; set; }
        public string Municipality { get; set; }
        public string Sector { get; set; }

        public virtual City City { get; set; }
    }
}
