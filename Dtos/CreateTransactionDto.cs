namespace CarWashManagementSystem.Dtos
{
    public class CreateTransactionDto
    {
        public bool WasFiscalized { get; set; }
        public short Value { get; set; }
        public short StationNumber { get; set; }
        public string StationTypeName { get; set; }
        public string TransactionSourceName { get; set; }
    }
}
