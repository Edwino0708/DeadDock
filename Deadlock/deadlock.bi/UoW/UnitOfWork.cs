using deadlock.bl.Repositories.Base;
using deadlock.data.Context;
using deadlock.data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace deadlock.bi.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DeadLockDbContext _context;

        private IRepository<Address> _address;
        private IRepository<City> _city;
        private IRepository<Country> _country;
        private IRepository<Contact> _contact;
        private IRepository<Person> _person;
        private IRepository<Employee> _employee;
        private IRepository<Position> _position;

        public UnitOfWork(DeadLockDbContext context)
        {
            _context = context;
        }

        public IRepository<Address> Address
        {
            get
            {
                if (_address == null)
                    _address = new Repository<Address>(_context);

                return _address;
            }
        }
        public IRepository<City> Cities
        {
            get
            {
                if (_city == null)
                    _city = new Repository<City>(_context);

                return _city;
            }
        }
        public IRepository<Country> Countries
        {
            get
            {
                if (_country == null)
                    _country = new Repository<Country>(_context);

                return _country;
            }
        }

        public IRepository<Contact> Contacts
        {
            get
            {
                if (_contact == null)
                    _contact = new Repository<Contact>(_context);

                return _contact;
            }
        }
        public IRepository<Person> Persons
        {
            get
            {
                if (_person == null)
                    _person = new Repository<Person>(_context);

                return _person;
            }
        }


        public IRepository<Employee> Employees
        {
            get
            {
                if (_employee == null)
                    _employee = new Repository<Employee>(_context);

                return _employee;
            }
        }
        public IRepository<Position> Positions
        {
            get
            {
                if (_position == null)
                    _position = new Repository<Position>(_context);

                return _position;
            }
        }

      
        public async Task<int> Commit()
        {
            return await _context.SaveChangesAsync();
        }

        public async void Dispose()
        {
            await _context.DisposeAsync();
        }

        public async void RejectChanges()
        {
            foreach (var entry in _context.ChangeTracker.Entries()
             .Where(e => e.State != EntityState.Unchanged))
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        await entry.ReloadAsync();
                        break;
                }
            }
        }
    }
}
