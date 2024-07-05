namespace CarWashManagementSystem.Models
{
    public class Station
    {
        public int Id { get; set; }
        public short StationNumber { get; set; }
        public int StationTypeId { get; set; }
        public StationType StationType { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}
