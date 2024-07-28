namespace CarWashManagementSystem.Dtos
{
    public class StationDto
    {
        public int Id { get; set; }
        public short StationNumber { get; set; }
        public StationTypeDto StationType { get; set; }
    }
}
