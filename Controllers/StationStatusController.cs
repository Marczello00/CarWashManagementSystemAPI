using CarWashManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;

namespace CarWashManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationStatusController : ControllerBase
    {
        private readonly DataContext _context;

        public StationStatusController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("{stationId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]

        public IActionResult GetStationStatus(int stationId)
        {
            //Aktualny dzień i czas
            var now = DateTime.Now;
            var currentDayOfWeek = now.DayOfWeek;
            var currentTime = now.TimeOfDay;

            var station = _context.Stations.Find(stationId);

            // Sprawdź czy stacja istnieje
            if (station == null)
                return NotFound("Station not found.");

            // Sprawdź czy stacja jest wyłączona z harmonogramu
            // Jeśli tak, zwróć manualny stan fiskalizacji
            if (station.IsExcludedFromSchedule)
                return Ok(new { StationId = stationId, IsFiscOn = station.ManualFiscState });

            // Znajdż harmonogram dla aktualnego dnia
            var schedules = _context.FiscSchedule
                .Where(s => s.DayOfWeek == currentDayOfWeek)
                .ToList();

            // Sprawdź czy fiskalizacja jest włączona w harmonogramie
            // Jeśli tak, zwróć stan włączony
            // Jeśli nie (Harmonogram:00:00:00-00:00:00), zwróć stan wyłączony
            var isOn = schedules.Any(s =>
                (s.TurnOnTime == TimeSpan.Zero && s.TurnOffTime == TimeSpan.Zero)
                ? false
                : (s.TurnOnTime <= currentTime && s.TurnOffTime >= currentTime));

            return Ok(new { StationId = stationId, IsFiscOn = isOn });
        }
    }
}
