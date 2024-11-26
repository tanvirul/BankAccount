using BankAccount.Domain.Entities;
using BankAccount.Domain.Repositories;
using BankAccount.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BankAccount.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _context;

        public TransactionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transaction>> GetMonthlyAccountStatements(string accountNumber, DateTime statementDate, CancellationToken cancellationToken)
        {
            var allTransactions = await _context.Transactions.AsNoTracking()
                  .Where(t => t.AccountNumber == accountNumber && (t.Date.Year == statementDate.Year && t.Date.Month == statementDate.Month))
                  .OrderBy(t => t.Date).ThenBy(t => t.Id)
                  .ToListAsync(cancellationToken);

            return allTransactions;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsAsync(string accountNumber, CancellationToken cancellationToken)
        {
            var allTransactions = await _context.Transactions.AsNoTracking()
                  .Where(t => t.AccountNumber == accountNumber)
                  .OrderBy(t => t.Date)
                  .ToListAsync(cancellationToken);

            return allTransactions;
        }
    }
}
