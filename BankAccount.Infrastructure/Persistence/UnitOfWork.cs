using BankAccount.Domain.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace BankAccount.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IAccountRepository AccountRepository { get; }
        public IInterestRuleRepository InterestRuleRepository { get; }
        public ITransactionRepository TransactionRepository { get; }
        private IDbContextTransaction _dbTransaction;

        public UnitOfWork(AppDbContext context,
            IAccountRepository accountRepository,
            ITransactionRepository transactionRepository,
            IInterestRuleRepository interestRuleRepository)
        {
            _context = context;
            AccountRepository = accountRepository;
            TransactionRepository = transactionRepository;
            InterestRuleRepository = interestRuleRepository;
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _dbTransaction = await _context.Database.BeginTransactionAsync();
        }
        public async Task CommitAsync()
        {
            if (_dbTransaction != null)
            {
                await _context.SaveChangesAsync();
                await _dbTransaction.CommitAsync();
            }
        }
        public async Task RollbackAsync()
        {
            if (_dbTransaction != null)
            {
                await _dbTransaction.RollbackAsync();
            }
        }
        public void Dispose()
        {
            _dbTransaction?.Dispose();
            _context.Dispose();
        }
    }

}
