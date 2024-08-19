namespace CarWashManagementSystem.Dtos
{
    public class MainPageDataDto
    {
        public int Id { get; set; }
        public string StationName { get; set; }
        public int TodaysRevenue { get; set; }
        public bool IsStationWorking { get; set; }
        public bool IsTaxingEnabled { get; set; }
        public string IpAddress { get; set; }
        public short StationNumber { get; set; }
        public string StationTypeName { get; set; }

    }
}
