using MediatR;

namespace BankAccount.Application.Commands.CreateTransaction
{
    public record CreateTransactionCommand(string Date, string AccountNumber, string Type, decimal Amount) : IRequest<Unit>;
}
