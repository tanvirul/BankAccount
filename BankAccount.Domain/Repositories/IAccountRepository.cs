using BankAccount.Domain.Entities;

namespace BankAccount.Domain.Repositories
{
    public interface IAccountRepository
    {
        Task<Account> GetAccountAsync(string accountNumber);
        Task<Account> GetAccountWithLock(string accountNumber);
        Task AddAccountAsync(Account account);
        Task<IEnumerable<Account>> GetAccountsWithLockAsync(int pageSize, int pageNumber);
    }

}
