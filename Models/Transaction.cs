namespace CarWashManagementSystem.Models
{
    public class Transaction
    {
        public long Id { get; set; }
        public DateTime DateTime { get; set; }
        public bool WasFiscalized { get; set; }
        public short Value { get; set; }
        public int StationId { get; set; }
        public Station Station { get; set; }
        public int TransactionSourceId { get; set; }
        public TransactionSource TransactionSource { get; set; }
    }
}
