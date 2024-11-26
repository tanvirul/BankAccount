using BankAccount.Domain.Entities;
using BankAccount.Domain.Entities.Exceptions;
using BankAccount.Domain.Repositories;
using MediatR;

namespace BankAccount.Application.Queries.GetAccountStatement
{
    public class GetAccountStatementQueryHandler : IRequestHandler<GetAccountStatementQuery, IEnumerable<Transaction>>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;
        public GetAccountStatementQueryHandler(ITransactionRepository transactionRepository, IAccountRepository accountRepository)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;

        }
        public async Task<IEnumerable<Transaction>> Handle(GetAccountStatementQuery request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetAccountAsync(request.AccountNumber);

            if (account == null)
            {
                throw new AccountNotFoundException("Account not found.");
            }
            return await _transactionRepository.GetMonthlyAccountStatements(request.AccountNumber, request.Date);
        }
    }
}
