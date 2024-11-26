using BankAccount.Domain.Entities;

namespace BankAccount.Domain.Repositories
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetTransactionsAsync(string accountNumber, CancellationToken cancellationToken = default);
        Task<IEnumerable<Transaction>> GetMonthlyAccountStatements(string accountNumber, DateTime statementDate, CancellationToken cancellationToken = default);
    }
}
