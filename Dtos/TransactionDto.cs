using CarWashManagementSystem.Models;

namespace CarWashManagementSystem.Dtos
{
    public class TransactionDto
    {
        public long Id { get; set; }
        public DateTime DateTime { get; set; }
        public bool WasFiscalized { get; set; }
        public short Value { get; set; }
        public StationDto Station { get; set; }
        public TransactionSourceDto TransactionSource { get; set; }
    }
}
