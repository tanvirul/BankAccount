namespace BankAccount.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; } // Primary key for EF
        public string TxnId { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public string AccountNumber { get; set; }
        public decimal BalanceAfterTransaction { get; set; }
        public Account Account { get; set; }
    }


}
