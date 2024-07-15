namespace CarWashManagementSystem.Models
{
    public class StationAllowedIp
    {
        public int Id { get; set; }
        public int StationId { get; set; }
        public string IpAddress { get; set; }
        public Station Station { get; set; }
    }
}
