using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using CarWashManagementSystem.Data;
using CarWashManagementSystem.Dtos;

namespace CarWashManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public StationsController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<StationInfoDto>))]
        [ProducesResponseType(204)]
        public IActionResult GetStations()
        {
            var stations = _context.Stations
                .Include(s => s.StationType)
                .ToList();
            var mapped = _mapper.Map<List<StationInfoDto>>(stations);
            if (mapped == null || (mapped.Count == 0))
            {
                return NoContent();
            }
            return Ok(mapped);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(StationInfoDto))]
        [ProducesResponseType(404)]
        public IActionResult UpdateStation(int id, UpdateStationDto updateStationDto)
        {
            var station = _context.Stations.FirstOrDefault(s => s.Id == id);

            if (station == null)
                return NotFound("Station with this Id not found!");

            _mapper.Map(updateStationDto, station);
            _context.SaveChanges();
            station = _context.Stations
                .Include(s => s.StationType)
                .FirstOrDefault(s => s.Id == id);
            var mapped = _mapper.Map<StationInfoDto>(station);
            return Ok(mapped);
        }
    }
}
