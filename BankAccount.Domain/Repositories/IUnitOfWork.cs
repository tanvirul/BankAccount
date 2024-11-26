namespace BankAccount.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountRepository AccountRepository { get; }
        IInterestRuleRepository InterestRuleRepository { get; }
        ITransactionRepository TransactionRepository { get; }
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task<int> SaveAsync();
    }

}
