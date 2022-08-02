namespace Bank.WebAPI.Backend.Models
{
    [Table("Transactions")]
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public string TransactionUniqueReference { get; set; } = string.Empty;
        public decimal TransactionAmount { get; set; }
        public TranStatus TransactionStatus { get; set; }
        public bool IsSuccessful => TransactionStatus.Equals(TranStatus.Success);
        public string TransactionSourceAccount { get; set; } = string.Empty;
        public string TransactionDestinationAccount { get; set; } = string.Empty;
        public string TransactionParticulars { get; set; } = string.Empty;
        public TranType TransactionType { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now;


        public Transaction()
        {
            TransactionUniqueReference = $"{Guid.NewGuid().ToString().Replace("-","").Substring(1, 27)}";
        }
    }

    public enum TranStatus
    {
        Failed,
        Success,
        Error
    }

    public enum TranType
    {
        Deposit,
        Withdraw,
        Transfer
    }
}
