namespace CarWashManagementSystem.Dtos
{
    public class StationStatusDto
    {
        public int Id { get; set; }
        public bool IsFiscOn { get; set; }
        public bool AreCashPaymentsAllowed { get; set; }
        public bool AreCardPaymentsAllowed { get; set; }
    }
}
