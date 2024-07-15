using CarWashManagementSystem.Data;
using CarWashManagementSystem.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace CarWashManagementSystem.Filters
{
    public class AllowStationIpAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var dbContext = context.HttpContext.RequestServices.GetService<DataContext>();
            var env = context.HttpContext.RequestServices.GetService<IHostEnvironment>();
            var ipAddress = context.HttpContext.Connection.RemoteIpAddress?.ToString();

            // Jeśli środowisko jest deweloperskie, zaakceptuj wszystkie żądania
            if (env.IsDevelopment())
            {
                base.OnActionExecuting(context);
                return;
            }
            if (string.IsNullOrEmpty(ipAddress))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Pobierz StationId z parametrów akcji
            if (!context.ActionArguments.TryGetValue("createTransactionDto", out var value) || value is not CreateTransactionDto createTransactionDto)
            {
                context.Result = new BadRequestResult();
                return;
            }

            var station = dbContext.Stations
                .Include(s => s.StationType)
                .FirstOrDefault(s => s.StationNumber == createTransactionDto.StationNumber &&
                                     s.StationType.StationTypeName == createTransactionDto.StationTypeName);

            if (station == null)
            {
                context.Result = new NotFoundResult();
                return;
            }
            // Sprawdź, czy adres IP klienta jest dozwolony dla określonej stacji
            var isAllowed = dbContext.StationAllowedIps
                .Any(ip => ip.StationId == station.Id && ip.IpAddress == ipAddress);

            if (!isAllowed)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
