using MediatR;

namespace BankAccount.Application.Commands.CreateAccount
{
    public record CreateAccountCommand(string AccountNumber) : IRequest<Unit>;
}
