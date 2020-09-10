using System;
using System.Collections.Generic;

namespace deadlock.data.Models
{
    public partial class Employee
    {
        public Guid Id { get; set; }
        public Guid PositionId { get; set; }
        public Guid PersonId { get; set; }
        public string EmplNumber { get; set; }
        public decimal Salary { get; set; }
        public bool Status { get; set; }
        public string EmailEmployee { get; set; }
        public Guid? Supervisor { get; set; }
        public DateTime HireDate { get; set; }

        public virtual Person Person { get; set; }
        public virtual Position Position { get; set; }
    }
}
