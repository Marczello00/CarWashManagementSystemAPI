namespace CarWashManagementSystem.Models
{
    public class StationType
    {
        public int Id { get; set; }
        public string StationTypeName { get; set; }
        public ICollection<Station> Stations { get; set; }
    }
}
