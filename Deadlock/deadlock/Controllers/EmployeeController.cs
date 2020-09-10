using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using deadlock.bi.UoW;
using deadlock.data.Models;
using deadlock.ModelDtos;
using deadlock.Models;
using deadlock.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace deadlock.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeController> _logger;
    
        public EmployeeController(IUnitOfWork unitOfWork,
             IMapper mapper,
             ILogger<EmployeeController> logger)
        {
            _db = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllEmployee(Guid id)
        {
            try
            {

                var employee = await _db.Employees.FindSingleByAsync(w => w.Id.Equals(id));


                if (employee == null)
                {
                    return NotFound();
                }

                var person = await _db.Persons.FindSingleByAsync(w => w.Id.Equals(employee.PersonId));
                var contact = await _db.Contacts.FindSingleByAsync(w => w.PersonId.Equals(person.Id));
                var address = await _db.Address.FindSingleByAsync(w => w.Id.Equals(person.AddressId));
                var city = await _db.Cities.FindSingleByAsync(w => w.Id.Equals(address.CityId));
                var country = await _db.Countries.FindSingleByAsync(w => w.Id.Equals(city.CountryId));

                var employeeDto = new CreateEmployeeDto();
                employeeDto.Id = employee.Id;
                employeeDto.PositionId = employee.PositionId;
                employeeDto.FirstName = person.FirstName;
                employeeDto.LastName = person.LastName;
                employeeDto.Gender = person.Gender;
                employeeDto.EmplNumber = employee.EmplNumber;
                employeeDto.Salary = employee.Salary.ToString();
                employeeDto.HireDate =  employee.HireDate.ToString();
                employeeDto.Supervisor = employee.Supervisor;
                employeeDto.DateOfBirth = person.DateOfBirth.ToString();
                employeeDto.Email = contact.Email;
                employeeDto.PhoneNumber = contact.PhoneNumber;
                employeeDto.MobileNumber = contact.MobileNumber;
                employeeDto.EmailEmployee = employee.EmailEmployee;
                employeeDto.StreetName = address.StreetName;
                employeeDto.HouseNumber = address.HouseNumber.ToString();
                employeeDto.Municipality = address.Municipality;
                employeeDto.Sector = address.Sector;
                employeeDto.City = city.Name;
                employeeDto.Country = country.Name;



                return Ok(employeeDto);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo una empleado by Id");
                return StatusCode(500);
            }
        }

        [HttpGet()]
        public async Task<IActionResult> GetPaged(int page, int size, string q)
        {
            try
            {
                var searchString = q?.ToUpper();

                var employees = _db.Employees.GetAllAsQueryable()
                    .Include(i => i.Person)
                    .Include(i => i.Position)
                    .AsQueryable();

                var count = await employees.CountAsync();

                if (!string.IsNullOrEmpty(q))
                {
                    employees = employees.Where(e =>
                        e.Person.FirstName != null && e.Person.FirstName.ToUpper().Contains(searchString) ||
                        e.Person.LastName != null && e.Person.LastName.ToUpper().Contains(searchString) ||
                        e.EmplNumber != null && e.EmplNumber.ToUpper().Contains(searchString) ||
                        e.Position.Name != null && e.Position.Name.ToUpper().Contains(searchString));
                }

                var countFlt = await employees.CountAsync();

                var employeeList = await employees.OrderBy(e => e.EmplNumber)
               .Skip(page * size)
               .Take(size)
               .ToListAsync();

                var mappedEmployees = _mapper.Map<HashSet<EmployeeDto>>(employeeList);

                return Ok(new PagedResponseVm<EmployeeDto>
                {
                    Data = mappedEmployees,
                    Count = countFlt,
                    Total = count
                });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo clientes");
                return StatusCode(500);
            }
        }


        [HttpPost()]
        public async Task<IActionResult> EmployeeData([FromBody] CreateEmployeeDto employeeDto) 
        {
            try 
            {
                if (employeeDto == null) 
                {
                    return BadRequest();
                }

                dynamic countryId = null;
                dynamic cityId = null; 
                var existCountry = await _db.Countries.FindSingleByAsync(w => w.Name.ToUpper().Contains(employeeDto.Country.ToUpper()));
                if (existCountry == null)
                {
                    var country = new Country();
                    country.Id = Guid.NewGuid();
                    country.Name = employeeDto.Country;
                    _db.Countries.Add(country);
                    await _db.Commit();
                    countryId = country.Id;

                }
                else {
                    countryId = existCountry.Id;
                }

                var existCity = await _db.Cities.FindSingleByAsync(w => w.Name.ToUpper().Contains(employeeDto.City.ToUpper()));
                if (existCity == null)
                {
                    var city = new City();
                    city.Id = Guid.NewGuid();
                    city.CountryId = countryId;
                    city.Name = employeeDto.City;
                    cityId = city.Id;
                    _db.Cities.Add(city);
                    await _db.Commit();
                }
                else
                {
                    cityId = existCity.Id;
                }


                var address = new Address();
                address.Id = Guid.NewGuid();
                address.CityId = cityId;
                address.StreetName = employeeDto.StreetName;
                address.HouseNumber = decimal.Parse(employeeDto.HouseNumber);
                address.Sector = employeeDto.Sector;
                address.Municipality = employeeDto.Municipality;
                
                _db.Address.Add(address);
                await _db.Commit();

              
                var person = new Person();
                person.Id = Guid.NewGuid();

                person.FirstName = employeeDto.FirstName;
                person.LastName = employeeDto.LastName;
                person.Gender = employeeDto.Gender;
                person.AddressId = address.Id;
                person.Status = true;
                person.DateOfBirth = DateTime.Parse(employeeDto.DateOfBirth);
                _db.Persons.Add(person);
                await _db.Commit();

                var contact = new Contact();
                contact.Id = Guid.NewGuid();
                contact.PersonId = person.Id;
                contact.MobileNumber = employeeDto.MobileNumber;
                contact.PhoneNumber = employeeDto.PhoneNumber;
                contact.Email = employeeDto.Email;
                _db.Contacts.Add(contact);
                await _db.Commit();

                var employee = new Employee();
                employee.Id = Guid.NewGuid();
                employee.PersonId = person.Id;
                employee.EmplNumber = employeeDto.EmplNumber;
                employee.Salary = decimal.Parse(employeeDto.Salary);
                employee.Status = true;
                employee.HireDate = DateTime.Now;
                employee.EmailEmployee = employeeDto.EmailEmployee;
                employee.PositionId = employeeDto.PositionId;

                _db.Employees.Add(employee);
                await _db.Commit();
               

                return NoContent();

            }catch (Exception ex) {
                _logger.LogError(ex, "Error guardando Employees");
                return StatusCode(500);
            }

        }

        [HttpPut()]
        public async Task<IActionResult> UpdateEmployee([FromBody] CreateEmployeeDto employeeDto)
        {
            try
            {
                if (employeeDto == null)
                {
                    return BadRequest();
                }

                var employee = await _db.Employees.FindSingleByAsync(w => w.Id.Equals(employeeDto.Id));

                if (employee == null)
                {
                    return NotFound();
                }
                var person = await _db.Persons.FindSingleByAsync(w => w.Id.Equals(employee.PersonId));
                var contact = await _db.Contacts.FindSingleByAsync(w => w.PersonId.Equals(person.Id));
                var address = await _db.Address.FindSingleByAsync(w => w.Id.Equals(person.AddressId));
                var city = await _db.Cities.FindSingleByAsync(w => w.Id.Equals(address.CityId));
                var country = await _db.Countries.FindSingleByAsync(w => w.Id.Equals(city.CountryId));


                country.Name = employeeDto.Country;

                _db.Countries.Update(country);
                await _db.Commit();

                city.CountryId = country.Id;
                city.Name = employeeDto.City;

                _db.Cities.Update(city);
                await _db.Commit();


                address.CityId = city.Id;
                address.StreetName = employeeDto.StreetName;
                address.HouseNumber = decimal.Parse(employeeDto.HouseNumber);
                address.Sector = employeeDto.Sector;
                address.Municipality = employeeDto.Municipality;

                _db.Address.Update(address);
                await _db.Commit();


          
                person.FirstName = employeeDto.FirstName;
                person.LastName = employeeDto.LastName;
                person.Gender = employeeDto.Gender;
                person.AddressId = address.Id;
                person.Status = true;
                person.DateOfBirth = DateTime.Parse(person.DateOfBirth.ToString());
                _db.Persons.Update(person);
                await _db.Commit();

                contact.PersonId = person.Id;
                contact.MobileNumber = employeeDto.MobileNumber;
                contact.PhoneNumber = employeeDto.PhoneNumber;
                contact.Email = employeeDto.Email;
                _db.Contacts.Update(contact);
                await _db.Commit();

                employee.PersonId = person.Id;
                employee.EmplNumber = employeeDto.EmplNumber;
                employee.Salary = decimal.Parse(employeeDto.Salary);
                employee.Status = true;
                employee.HireDate = DateTime.Now;
                employee.PositionId = employeeDto.PositionId;

                _db.Employees.Update(employee);
                await _db.Commit();


                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando Employees");
                return StatusCode(500);
            }

        }



        //Metodo si se desea eliminar de la base de dato
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(Guid id) 
        {
            try {

                var existEmployee = await _db.Employees.FindSingleByAsync(w => w.Id.Equals(id));
                
                if (existEmployee == null) {
                    return NotFound();
                }
                _db.Employees.Delete(existEmployee);
                await _db.Commit();
                
                var contact = await _db.Contacts.FindSingleByAsync(w => w.PersonId.Equals(existEmployee.PersonId));
                _db.Contacts.Delete(contact);
                await _db.Commit();

                var person = await _db.Persons.FindSingleByAsync(w => w.Id.Equals(existEmployee.PersonId));
                _db.Persons.Delete(person);
                await _db.Commit();

                var address = await _db.Address.FindSingleByAsync(w => w.Id.Equals(person.AddressId));
                _db.Address.Delete(address);
                await _db.Commit();

                return NoContent();

            }catch (Exception ex) {
                _logger.LogError(ex, "Error al eliminar una Empleado");
                return StatusCode(500);
            }
        }

        [HttpDelete("status/{id}")]
        public async Task<IActionResult> DeleteEmployeeStatus(Guid id)
        {
            try
            {

                var existEmployee = await _db.Employees.FindSingleByAsync(w => w.Id.Equals(id));


                if (existEmployee == null)
                {
                    return NotFound();
                }

                existEmployee.Status = false;

                _db.Employees.Update(existEmployee);
                await _db.Commit();

                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar una Empleado");
                return StatusCode(500);
            }
        }
    }
}
