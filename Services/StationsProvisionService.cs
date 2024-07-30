﻿using CarWashManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using CarWashManagementSystem.Interfaces;

namespace CarWashManagementSystem.Services
{
    public class StationsProvisionService : IStationService
    {
        private readonly DataContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<StationsProvisionService> _logger;
        public StationsProvisionService(DataContext context, IHttpClientFactory httpClientFactory, ILogger<StationsProvisionService> logger)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }
        public async Task ProvisionActiveStationsAsync()
        {
            var activeStations = await _context.Stations
            .Include(s => s.AllowedIp)
            .Where(s => s.IsActive)
            .ToListAsync();

            var tasks = new List<Task>();

            foreach (var station in activeStations)
            {
                if (station.AllowedIp != null)
                {
                    var url = $"http://{station.AllowedIp.IpAddress}/provision";
                    tasks.Add(CallProvisionEndpointAsync(url));
                }
            }

            await Task.WhenAll(tasks);
        }
        private async Task CallProvisionEndpointAsync(string url)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Failed to provision station at {url}. Status Code: {response.StatusCode}");
            }

            response.EnsureSuccessStatusCode();
        }
    }
}