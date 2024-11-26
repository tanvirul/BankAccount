using BankAccount.Domain.Entities.Exceptions;

namespace BankAccount.Domain.Entities
{
    public class Account
    {
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        public void AddTransaction(Transaction transaction)
        {
            if (transaction.Type.ToUpper() == "W" && Balance - transaction.Amount < 0)
            {
                throw new InsufficientFundException("Insufficient funds for withdrawal.");
            }

            Balance += transaction.Type.ToUpper() == "D" || transaction.Type.ToUpper() == "I" ? transaction.Amount : -transaction.Amount;
            transaction.BalanceAfterTransaction = Balance;
            Transactions.Add(transaction);
        }
    }

}
