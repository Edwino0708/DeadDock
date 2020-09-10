using System;
using System.Collections.Generic;

namespace deadlock.data.Models
{
    public partial class Contact
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }

        public virtual Person Person { get; set; }
    }
}
