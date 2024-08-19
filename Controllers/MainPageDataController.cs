using AutoMapper;
using CarWashManagementSystem.Data;
using CarWashManagementSystem.Dtos;
using CarWashManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace CarWashManagementSystem.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class MainPageDataController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<MainPageDataController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        public MainPageDataController(DataContext context, IMapper mapper, ILogger<MainPageDataController> logger, IHttpClientFactory clientFactory)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _clientFactory = clientFactory;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MainPageDataDto>))]
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetMainPageDataAsync()
        {
            var stations = _context.Stations
                .Where(s => s.IsActive == true)
                .Include(s => s.AllowedIp)
                .Include(s => s.StationType)
                .AsNoTracking()
                .ToList();

            var mapped = _mapper.Map<List<MainPageDataDto>>(stations);
            if (mapped == null || (mapped.Count == 0))
            {
                return NoContent();
            }


            var tasks = new List<Task>();

            foreach (var station in mapped)
            {
                var todaysRevenue = _context.Transactions
                    .Where(t => t.StationId == station.Id && t.DateTime.Date == DateTime.Now.Date)
                    .Sum(t => t.Value);
                station.TodaysRevenue = todaysRevenue;

                var stationName = station.StationTypeName[0] + station.StationNumber.ToString();
                station.StationName = stationName;

                var task = Task.Run(async () =>
                {
                    try
                    {
                        StationResponseDto? stationAttributes = await GetStationAttributes(station.IpAddress);
                        if (stationAttributes != null)
                        {
                            station.IsStationWorking = stationAttributes.isCarWashWorking;
                            station.IsTaxingEnabled = stationAttributes.isTaxingEnabled;
                        }
                        else
                        {
                            station.IsStationWorking = false;
                            station.IsTaxingEnabled = false;
                        }
                    }
                    catch (Exception)
                    {
                        _logger.LogError("Error while getting station attributes for IP: {Ip}", station.IpAddress);
                        station.IsStationWorking = false;
                        station.IsTaxingEnabled = false;
                    }
                });
                tasks.Add(task);
            }
            await Task.WhenAll(tasks);
            return Ok(mapped);
        }
        private async Task<StationResponseDto?> GetStationAttributes(string Ip)
        {
            var client = _clientFactory.CreateClient("nonStandardHttpClient");
            client.Timeout = TimeSpan.FromSeconds(5);
            var url = $"http://{Ip}/myStatus";
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<StationResponseDto>(await response.Content.ReadAsStringAsync());
            }
            return null;
        }
    }
}
