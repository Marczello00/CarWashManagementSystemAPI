namespace CarWashManagementSystem.Models
{
    public class TransactionSource
    {
        public int Id { get; set; }
        public string SourceName { get; set; }
        public bool ShouldBeFiscalized { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}
