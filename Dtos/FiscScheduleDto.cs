namespace CarWashManagementSystem.Dtos
{
    public class FiscScheduleDto
    {
        public int Id { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan TurnOnTime { get; set; }
        public TimeSpan TurnOffTime { get; set; }
    }
}
