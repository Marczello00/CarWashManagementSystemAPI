﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using CarWashManagementSystem.Data;
using CarWashManagementSystem.Dtos;
using CarWashManagementSystem.Filters;
using Microsoft.AspNetCore.Authorization;
using CarWashManagementSystem.Constants;

namespace CarWashManagementSystem.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    [Authorize]
    public class StationAllowedIpController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public StationAllowedIpController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        [Authorize(Roles = UserRoles.Admin)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<StationAllowedIpDto>))]
        [ProducesResponseType(204)]
        public IActionResult GetStationAllowedIps()
        {
            var stationAllowedIps = _context.StationAllowedIps.ToList();
            var mapped = _mapper.Map<List<StationAllowedIpDto>>(stationAllowedIps);
            if (mapped == null || (mapped.Count == 0))
            {
                return NoContent();
            }
            return Ok(mapped);
        }
        [HttpPut("{StationId}")]
        [Authorize(Roles = UserRoles.Admin)]
        [ServiceFilter(typeof(ProvisioningActionFilter))]
        [ProducesResponseType(200, Type = typeof(StationAllowedIpDto))]
        [ProducesResponseType(404)]
        public IActionResult UpdateStationAllowedIp(int StationId, UpdateStationAllowedIpDto updateStationAllowedIpDto)
        {
            var stationAllowedIp = _context.StationAllowedIps.FirstOrDefault(s => s.StationId == StationId);

            if (stationAllowedIp == null)
                return NotFound("Station with this Id not found!");

            _mapper.Map(updateStationAllowedIpDto, stationAllowedIp);
            _context.SaveChanges();
            stationAllowedIp = _context.StationAllowedIps.FirstOrDefault(s => s.StationId == StationId);
            var mapped = _mapper.Map<StationAllowedIpDto>(stationAllowedIp);
            return Ok(mapped);
        }
    }
}
