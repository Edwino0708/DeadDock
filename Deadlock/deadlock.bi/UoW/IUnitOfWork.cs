using deadlock.bl.Repositories.Base;
using deadlock.data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace deadlock.bi.UoW
{
    public interface IUnitOfWork
    {
        IRepository<Address> Address { get; }
        IRepository<City> Cities { get; }
        IRepository<Country> Countries { get; }
        IRepository<Contact> Contacts { get; }
        IRepository<Person> Persons { get; }
        IRepository<Employee> Employees { get; }
        IRepository<Position> Positions { get; }
       
        Task<int> Commit();
        void RejectChanges();
        void Dispose();
    }
}
