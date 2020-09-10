using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deadlock.Models.Dtos
{
    public class CreateEmployeeDto
    {

        public Guid? Id { get; set; }
        public Guid PositionId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string EmplNumber { get; set; }
        public string Salary { get; set; }
        public string EmailEmployee { get; set; }
        public Guid? Supervisor { get; set; }
        public string HireDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string Municipality { get; set; }
        public string Sector { get; set; }

        public string Country { get; set; }
        public string City { get; set; }
    }

    public class EmployeeDto
    {

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmplNumber { get; set; }

        public string Position { get; set; }
        public string Salary { get; set; }
        public DateTime HireDate { get; set; }
    }
}
