using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using CarWashManagementSystem.Data;
using CarWashManagementSystem.Dtos;
using CarWashManagementSystem.Interfaces;
using CarWashManagementSystem.Filters;

namespace CarWashManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IStationService _stationService;
        public StationsController(DataContext context, IMapper mapper, IStationService stationService)
        {
            _context = context;
            _mapper = mapper;
            _stationService = stationService;
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
        [ServiceFilter(typeof(ProvisioningActionFilter))]
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

        [HttpPut("make-provisions")]
        [ProducesResponseType(200)]
        [ProducesResponseType(502)]
        public async Task<IActionResult> MakeProvisions()
        {
            try
            {
                await _stationService.ProvisionActiveStationsAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(502, "Błąd przy provisioningu. Czy wszystkie stanowiska działają?");
            }
        }
    }
}
