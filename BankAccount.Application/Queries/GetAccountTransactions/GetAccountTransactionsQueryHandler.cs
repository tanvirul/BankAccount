using BankAccount.Application.Queries.GetAccountStatement;
using BankAccount.Domain.Entities;
using BankAccount.Domain.Entities.Exceptions;
using BankAccount.Domain.Repositories;
using MediatR;

namespace BankAccount.Application.Queries.GetAccountTransactions
{
    public class GetAccountTransactionsQueryHandler : IRequestHandler<GetAccountTransactionsQuery, IEnumerable<Transaction>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;

        public GetAccountTransactionsQueryHandler(IAccountRepository accountRepository, ITransactionRepository transactionRepository)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<IEnumerable<Transaction>> Handle(GetAccountTransactionsQuery request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetAccountAsync(request.AccountNumber);

            if (account == null)
            {
                throw new AccountNotFoundException("Account not found.");
            }
            return await _transactionRepository.GetTransactionsAsync(request.AccountNumber);
        }
    }
}
