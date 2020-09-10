using System;
using System.Collections.Generic;

namespace deadlock.data.Models
{
    public partial class Person
    {
        public Person()
        {
            Contact = new HashSet<Contact>();
            Employee = new HashSet<Employee>();
        }

        public Guid Id { get; set; }
        public Guid AddressId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public Boolean Status { get; set; }
        public DateTime DateOfBirth { get; set; }

        public virtual ICollection<Contact> Contact { get; set; }
        public virtual ICollection<Employee> Employee { get; set; }
    }
}
