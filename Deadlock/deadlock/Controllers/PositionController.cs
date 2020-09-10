using AutoMapper;
using deadlock.bi.UoW;
using deadlock.data.Models;
using deadlock.ModelDtos;
using deadlock.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deadlock.Controllers
{
    [Route("api/positions")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;
        private readonly ILogger<PositionController> _logger;
    
        public PositionController(IUnitOfWork unitOfWork,
             IMapper mapper,
             ILogger<PositionController> logger)
        {
            _db = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllPosition(Guid id)
        {
            try
            {

                var position = await _db.Positions.FindSingleByAsync(w => w.Id.Equals(id));


                if (position == null) {
                    return NotFound();
                }

                var positionDto = _mapper.Map<PositionDto>(position);

                return Ok(positionDto);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo una posicion by Id");
                return StatusCode(500);
            }
        }

        [HttpGet("allPositions")]
        public async Task<IActionResult> GetAllPosition() 
        {
            try {

                var positions = await _db.Positions.FindByConditionAsync(w => w.Status == true);
                var positionsDto = _mapper.Map<HashSet<PositionDto>>(positions);


                return Ok(positionsDto);

            } catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo posiciones");
                return StatusCode(500);
            }
        }

        [HttpGet()]
        public async Task<IActionResult> GetPaged(int page, int size, string q)
        {
            try
            {
                var searchString = q?.ToUpper();

                var positions = _db.Positions.GetAllAsQueryable()
                    .AsQueryable();

                var count = await positions.CountAsync();
              
                if (!string.IsNullOrEmpty(q))
                {
                    positions = positions.Where(e =>
                        e.Name != null && e.Name.ToUpper().Contains(searchString) ||
                        e.Description != null && e.Description.ToUpper().Contains(searchString));
                }

                var countFlt = await positions.CountAsync();

                var positionList = await positions.OrderBy(e => e.Name)
               .Skip(page * size)
               .Take(size)
               .ToListAsync();

                var mappedPositions = _mapper.Map<HashSet<PositionDto>>(positionList);

                return Ok(new PagedResponseVm<PositionDto>
                {
                    Data = mappedPositions,
                    Count = countFlt,
                    Total = count
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo posiciones");
                return StatusCode(500);
            }
        }


        [HttpPost()]
        public async Task<IActionResult> SavePostion([FromBody]PositionDto positionDto) 
        {
            try 
            {
                if (positionDto == null) 
                {
                    return BadRequest();
                }

                var pos = new Position();
                pos.Id = Guid.NewGuid();
                pos.Name = positionDto.Name;
                pos.Description = positionDto.Description;
                pos.Status = true;
                pos.CreationDate = DateTime.Now;

                _db.Positions.Add(pos);
                await _db.Commit();
               

                return NoContent();

            }catch (Exception ex) {
                _logger.LogError(ex, "Error guardando posición");
                return StatusCode(500);
            }

        }

        [HttpPut()]
        public async Task<IActionResult> UpdatePosition([FromBody] PositionDto positionDto)
        {
            try
            {

                var position = await _db.Positions.FindSingleByAsync(w => w.Id.Equals(positionDto.Id));


                if (position == null)
                {
                    return NotFound();
                }

                position.Name = positionDto.Name;
                position.Description = positionDto.Description;

                if (positionDto.Status.Equals("true"))
                {
                    position.Status = true;
                }
                else {
                    position.Status = false;
                }


                _db.Positions.Update(position);
                await _db.Commit();


                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando posición");
                return StatusCode(500);
            }

        }


        //Metodo si se desea eliminar de la base de dato
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePosition(Guid id) 
        {
            try {

                
                var employee = await _db.Employees.FindByConditionAsync(w => w.PositionId.Equals(id));

                if (employee.Any()) 
                {
                    foreach (var item in employee) 
                    {
                        _db.Employees.Delete(item);
                        await _db.Commit();

                        var contact = await _db.Contacts.FindSingleByAsync(w => w.PersonId.Equals(item.PersonId));
                        _db.Contacts.Delete(contact);
                        await _db.Commit();

                        var person = await _db.Persons.FindSingleByAsync(w => w.Id.Equals(item.PersonId));
                        _db.Persons.Delete(person);
                        await _db.Commit();

                        var address = await _db.Address.FindSingleByAsync(w => w.Id.Equals(person.AddressId));
                        _db.Address.Delete(address);
                        await _db.Commit();

                    }

                }

                var existPosition = await _db.Positions.FindSingleByAsync(w => w.Id.Equals(id));
                if (existPosition == null) 
                {
                    return NotFound();
                }
                _db.Positions.Delete(existPosition);
                await _db.Commit();


                return NoContent();

            }catch (Exception ex) {
                _logger.LogError(ex, "Error al eliminar una posición");
                return StatusCode(500);
            }
        }

        [HttpDelete("status/{id}")]
        public async Task<IActionResult> DeletePositionStatus(Guid id)
        {
            try
            {

                var existPosition = await _db.Positions.FindSingleByAsync(w => w.Id.Equals(id));


                if (existPosition == null)
                {
                    return NotFound();
                }

                existPosition.Status = false;

                _db.Positions.Update(existPosition);
                await _db.Commit();

                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar una posición");
                return StatusCode(500);
            }
        }
    }
}
