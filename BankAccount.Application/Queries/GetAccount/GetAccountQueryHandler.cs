using BankAccount.Domain.Entities;
using BankAccount.Domain.Repositories;
using MediatR;

namespace BankAccount.Application.Queries.GetAccount
{
    public class GetAccountQueryHandler : IRequestHandler<GetAccountQuery, Account>
    {
        private readonly IAccountRepository _accountRepository;
        public GetAccountQueryHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public async Task<Account> Handle(GetAccountQuery request, CancellationToken cancellationToken)
        {
            return await _accountRepository.GetAccountAsync(request.AccountNumber);
        }
    }
}
