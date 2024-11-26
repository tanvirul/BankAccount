using BankAccount.Domain.Entities;
using BankAccount.Domain.Repositories;
using BankAccount.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BankAccount.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _context;

        public AccountRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Account> GetAccountAsync(string accountNumber)
        {
            return await _context.Accounts.AsNoTracking().FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
        }
        public async Task<Account> GetAccountWithLock(string accountNumber)
        {
            return await _context.Accounts
            .FromSqlRaw("SELECT * FROM Account WITH (UPDLOCK, ROWLOCK) WHERE AccountNumber = {0}", accountNumber)
            .FirstOrDefaultAsync();
        }

        public async Task AddAccountAsync(Account account)
        {
            await _context.Accounts.AddAsync(account);
        }

        public async Task<IEnumerable<Account>> GetAccountsWithLockAsync(int pageSize, int pageNumber)
        {
            var skip = (pageNumber - 1) * pageSize;

            return await _context.Accounts
                .FromSqlRaw(@"
            SELECT * 
            FROM Account WITH (UPDLOCK, ROWLOCK)
            ORDER BY AccountNumber
            OFFSET {0} ROWS
            FETCH NEXT {1} ROWS ONLY", skip, pageSize)
                .ToListAsync();
        }
    }

}
