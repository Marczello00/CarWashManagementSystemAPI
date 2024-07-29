namespace CarWashManagementSystem.Models
{
    public class Station
    {
        public int Id { get; set; }
        public short StationNumber { get; set; }
        public int StationTypeId { get; set; }
        public bool IsExcludedFromSchedule { get; set; }
        public bool ManualFiscState { get; set; }
        public bool IsActive { get; set; }
        public StationType StationType { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public StationAllowedIp AllowedIp { get; set; }
    }
}
