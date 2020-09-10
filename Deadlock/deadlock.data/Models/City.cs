using System;
using System.Collections.Generic;

namespace deadlock.data.Models
{
    public partial class City
    {
        public City()
        {
            Address = new HashSet<Address>();
        }

        public Guid Id { get; set; }
        public Guid CountryId { get; set; }
        public string Name { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<Address> Address { get; set; }
    }
}
