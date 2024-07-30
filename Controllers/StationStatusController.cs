using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using CarWashManagementSystem.Data;
using CarWashManagementSystem.Dtos;

namespace CarWashManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationStatusController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public StationStatusController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(StationStatusDto))]
        [ProducesResponseType(404)]
        public IActionResult GetStationStatus(short stationNumber, string stationTypeName)
        {
            var station = _context.Stations
                .Include(s => s.StationType)
                .FirstOrDefault(s => s.StationNumber == stationNumber && s.StationType.StationTypeName == stationTypeName);

            // Sprawdź czy stacja istnieje
            if (station == null)
                return NotFound("Station not found.");

            var mapped = _mapper.Map<StationStatusDto>(station);

            if (!station.IsExcludedFromSchedule)
                mapped.IsFiscOn = CalculateFiscStateFromSchedule();

            return Ok(mapped);
        }
        private bool CalculateFiscStateFromSchedule()
        {
            //Aktualny dzień i czas
            var now = DateTime.Now;
            var currentDayOfWeek = now.DayOfWeek;
            var currentTime = now.TimeOfDay;

            // Znajdż harmonogram dla aktualnego dnia
            var schedules = _context.FiscSchedule
                .Where(s => s.DayOfWeek == currentDayOfWeek)
                .ToList();

            // Sprawdź czy fiskalizacja jest włączona w harmonogramie
            // Jeśli tak, zwróć stan włączony
            // Jeśli nie (Harmonogram->00:00:00-00:00:00), zwróć stan wyłączony
            return schedules.Any(s =>
                (s.TurnOnTime == TimeSpan.Zero && s.TurnOffTime == TimeSpan.Zero)
                ? false
                : (s.TurnOnTime <= currentTime && s.TurnOffTime >= currentTime));
        }
    }
}
