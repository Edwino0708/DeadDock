using System;
using System.Collections.Generic;

namespace deadlock.data.Models
{
    public partial class Position
    {
        public Position()
        {
            Employee = new HashSet<Employee>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual ICollection<Employee> Employee { get; set; }
    }
}
