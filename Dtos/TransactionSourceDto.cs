namespace CarWashManagementSystem.Dtos
{
    public class TransactionSourceDto
    {
        public int Id { get; set; }
        public string SourceName { get; set; }
        public bool ShouldBeFiscalized { get; set; }
    }
}
