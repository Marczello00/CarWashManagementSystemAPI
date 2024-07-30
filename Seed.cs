using CarWashManagementSystem.Models;
using CarWashManagementSystem.Data;
namespace CarWashManagementSystem
{
    public class Seed
    {
        private readonly DataContext dataContext;
        public Seed(DataContext context)
        {
            this.dataContext = context;
        }
        public void SeedDataContext()
        {
            if (!dataContext.FiscSchedule.Any())
            {
                dataContext.FiscSchedule.AddRange(
                    new FiscSchedule { DayOfWeek = DayOfWeek.Monday, TurnOnTime = new TimeSpan(8, 0, 0), TurnOffTime = new TimeSpan(17, 0, 0) },
                    new FiscSchedule { DayOfWeek = DayOfWeek.Tuesday, TurnOnTime = new TimeSpan(8, 0, 0), TurnOffTime = new TimeSpan(17, 0, 0) },
                    new FiscSchedule { DayOfWeek = DayOfWeek.Wednesday, TurnOnTime = new TimeSpan(8, 0, 0), TurnOffTime = new TimeSpan(17, 0, 0) },
                    new FiscSchedule { DayOfWeek = DayOfWeek.Thursday, TurnOnTime = new TimeSpan(8, 0, 0), TurnOffTime = new TimeSpan(17, 0, 0) },
                    new FiscSchedule { DayOfWeek = DayOfWeek.Friday, TurnOnTime = new TimeSpan(8, 0, 0), TurnOffTime = new TimeSpan(17, 0, 0) },
                    new FiscSchedule { DayOfWeek = DayOfWeek.Saturday, TurnOnTime = new TimeSpan(0, 0, 0), TurnOffTime = new TimeSpan(0, 0, 0) },
                    new FiscSchedule { DayOfWeek = DayOfWeek.Sunday, TurnOnTime = new TimeSpan(0, 0, 0), TurnOffTime = new TimeSpan(0, 0, 0) }
                );
            }
            if (!dataContext.StationTypes.Any())
            {
                dataContext.StationTypes.AddRange(
                    new StationType { Id = 1, StationTypeName = "Myjnia" },
                    new StationType { Id = 2, StationTypeName = "Odkurzacz" },
                    new StationType { Id = 3, StationTypeName = "Akcesoria" }
                );
            }
            if (!dataContext.Stations.Any())
            {
                dataContext.Stations.AddRange(
                    new Station { Id = 1, StationNumber = 1, StationTypeId = 1, IsExcludedFromSchedule = false, ManualFiscState = false, IsActive = false, AreCashPaymentsAllowed = true, AreCardPaymentsAllowed = true },
                    new Station { Id = 2, StationNumber = 2, StationTypeId = 1, IsExcludedFromSchedule = false, ManualFiscState = false, IsActive = false, AreCashPaymentsAllowed = true, AreCardPaymentsAllowed = true },
                    new Station { Id = 3, StationNumber = 3, StationTypeId = 1, IsExcludedFromSchedule = false, ManualFiscState = false, IsActive = true, AreCashPaymentsAllowed = true, AreCardPaymentsAllowed = true },
                    new Station { Id = 4, StationNumber = 4, StationTypeId = 1, IsExcludedFromSchedule = false, ManualFiscState = false, IsActive = false, AreCashPaymentsAllowed = true, AreCardPaymentsAllowed = true },
                    new Station { Id = 5, StationNumber = 1, StationTypeId = 2, IsExcludedFromSchedule = false, ManualFiscState = false, IsActive = false, AreCashPaymentsAllowed = true, AreCardPaymentsAllowed = true },
                    new Station { Id = 6, StationNumber = 2, StationTypeId = 2, IsExcludedFromSchedule = false, ManualFiscState = false, IsActive = false, AreCashPaymentsAllowed = true, AreCardPaymentsAllowed = true },
                    new Station { Id = 7, StationNumber = 1, StationTypeId = 3, IsExcludedFromSchedule = false, ManualFiscState = false, IsActive = false, AreCashPaymentsAllowed = true, AreCardPaymentsAllowed = true },
                    new Station { Id = 8, StationNumber = 2, StationTypeId = 3, IsExcludedFromSchedule = false, ManualFiscState = false, IsActive = false, AreCashPaymentsAllowed = true, AreCardPaymentsAllowed = true },
                    new Station { Id = 9, StationNumber = 3, StationTypeId = 3, IsExcludedFromSchedule = false, ManualFiscState = false, IsActive = false, AreCashPaymentsAllowed = true, AreCardPaymentsAllowed = true }
                );
            }
            if (!dataContext.TransactionSources.Any())
            {
                dataContext.TransactionSources.AddRange(
                    new TransactionSource { Id = 1, SourceName = "Gotówka", ShouldBeFiscalized = true },
                    new TransactionSource { Id = 2, SourceName = "Żeton", ShouldBeFiscalized = false },
                    new TransactionSource { Id = 3, SourceName = "Karta", ShouldBeFiscalized = true },
                    new TransactionSource { Id = 4, SourceName = "Prepaid", ShouldBeFiscalized = false },
                    new TransactionSource { Id = 5, SourceName = "Aplikacja", ShouldBeFiscalized = false }
                );
            }
            if (!dataContext.StationAllowedIps.Any())
            {
                dataContext.StationAllowedIps.AddRange(
                    new StationAllowedIp { Id = 1, StationId = 1, IpAddress = "192.168.1.30" },
                    new StationAllowedIp { Id = 2, StationId = 2, IpAddress = "192.168.1.30" },
                    new StationAllowedIp { Id = 3, StationId = 3, IpAddress = "192.168.1.30" },
                    new StationAllowedIp { Id = 4, StationId = 4, IpAddress = "192.168.1.30" },
                    new StationAllowedIp { Id = 5, StationId = 5, IpAddress = "192.168.1.30" },
                    new StationAllowedIp { Id = 6, StationId = 6, IpAddress = "192.168.1.30" },
                    new StationAllowedIp { Id = 7, StationId = 7, IpAddress = "192.168.1.30" },
                    new StationAllowedIp { Id = 8, StationId = 8, IpAddress = "192.168.1.30" },
                    new StationAllowedIp { Id = 9, StationId = 9, IpAddress = "192.168.1.30" }
                );
            }
            dataContext.SaveChanges();
        }
    }
}
