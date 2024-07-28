using AutoMapper;
using CarWashManagementSystem.Data;
using CarWashManagementSystem.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarWashManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public StationController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<StationInfoDto>))]
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
    }
}
