using CarWashManagementSystem.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CarWashManagementSystem.Filters
{
    public class ProvisioningActionFilter : IAsyncActionFilter
    {
        private readonly IStationService _stationService;
        private readonly ILogger<ProvisioningActionFilter> _logger;

        public ProvisioningActionFilter(IStationService stationService, ILogger<ProvisioningActionFilter> logger)
        {
            _stationService = stationService;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            try
            {
                await _stationService.ProvisionActiveStationsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during provisioning: {ex.Message}");
            }
        }
    }
}
