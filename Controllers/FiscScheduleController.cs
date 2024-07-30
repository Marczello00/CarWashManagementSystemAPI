using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using CarWashManagementSystem.Data;
using CarWashManagementSystem.Dtos;
using CarWashManagementSystem.Filters;

namespace CarWashManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FiscScheduleController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public FiscScheduleController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FiscScheduleDto>))]
        [ProducesResponseType(204)]
        public IActionResult GetFiscSchedule()
        {
            var fiscSchedule = _context.FiscSchedule.ToList();
            var mapped = _mapper.Map<List<FiscScheduleDto>>(fiscSchedule);
            if (mapped == null || (mapped.Count == 0))
            {
                return NoContent();
            }
            return Ok(mapped);
        }

        [HttpPut("{DayOfWeek}")]
        [ServiceFilter(typeof(ProvisioningActionFilter))]
        [ProducesResponseType(200, Type = typeof(FiscScheduleDto))]
        [ProducesResponseType(404)]
        public IActionResult UpdateFiscSchedule(DayOfWeek DayOfWeek, UpdateFiscScheduleDto updateFiscScheduleDto)
        {
            var fiscSchedule = _context.FiscSchedule.FirstOrDefault(s => s.DayOfWeek == DayOfWeek);

            if (fiscSchedule == null)
                return NotFound("Wrong day of week!");

            //cut off seconds
            updateFiscScheduleDto.TurnOnTime = new TimeSpan(updateFiscScheduleDto.TurnOnTime.Hours, updateFiscScheduleDto.TurnOnTime.Minutes, 0);
            updateFiscScheduleDto.TurnOffTime = new TimeSpan(updateFiscScheduleDto.TurnOffTime.Hours, updateFiscScheduleDto.TurnOffTime.Minutes, 0);

            //both time cannot be more than 23:59
            if (updateFiscScheduleDto.TurnOnTime > TimeSpan.FromHours(23) || updateFiscScheduleDto.TurnOffTime > TimeSpan.FromHours(23))
                return BadRequest("Time cannot be more than 23:59");

            //if turnon time is greater than turnoff time bad request
            if (updateFiscScheduleDto.TurnOnTime > updateFiscScheduleDto.TurnOffTime)
                return BadRequest("TurnOnTime cannot be greater than TurnOffTime");

            // both times could be equal only when thery are 00:00
            if (updateFiscScheduleDto.TurnOnTime == updateFiscScheduleDto.TurnOffTime && updateFiscScheduleDto.TurnOnTime != TimeSpan.Zero)
                return BadRequest("TurnOnTime and TurnOffTime cannot be equal unless its 00:00");

            _mapper.Map(updateFiscScheduleDto, fiscSchedule);
            _context.SaveChanges();

            fiscSchedule = _context.FiscSchedule
                .FirstOrDefault(s => s.DayOfWeek == DayOfWeek);

            var mapped = _mapper.Map<FiscScheduleDto>(fiscSchedule);

            return Ok(mapped);
        }
    }
}
