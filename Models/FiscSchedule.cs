namespace CarWashManagementSystem.Models
{
    public class FiscSchedule
    {
        public int Id { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan TurnOnTime { get; set; }
        public TimeSpan TurnOffTime { get; set; }
    }
}
