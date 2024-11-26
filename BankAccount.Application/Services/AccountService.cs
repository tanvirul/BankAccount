using BankAccount.Application.Commands.CreateAccount;
using BankAccount.Application.Queries.GetAccount;
using MediatR;

namespace BankAccount.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMediator _mediator;

        public AccountService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task EnsureAccountExistsAsync(string accountNumber)
        {
            // Check if the account exists
            var account = await _mediator.Send(new GetAccountQuery(accountNumber));

            // If the account doesn't exist, create it
            if (account == null)
            {
                await _mediator.Send(new CreateAccountCommand(accountNumber));
            }
        }
    }

}
