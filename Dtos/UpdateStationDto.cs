namespace CarWashManagementSystem.Dtos
{
    public class UpdateStationDto
    {
        public bool? IsExcludedFromSchedule { get; set; }
        public bool? ManualFiscState { get; set; }
        public bool? IsActive { get; set; }
        public bool? AreCashPaymentsAllowed { get; set; }
        public bool? AreCardPaymentsAllowed { get; set; }
    }
}
