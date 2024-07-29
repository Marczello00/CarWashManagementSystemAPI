namespace CarWashManagementSystem.Dtos
{
    public class StationInfoDto
    {
        public int Id { get; set; }
        public short StationNumber { get; set; }
        public bool IsExcludedFromSchedule { get; set; }
        public bool ManualFiscState { get; set; }
        public bool IsActive { get; set; }
        public StationTypeDto StationType { get; set; }
    }
}
