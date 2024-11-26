using BankAccount.Domain.Entities;
using MediatR;

namespace BankAccount.Application.Queries.GetAccountStatement
{
    public record GetAccountTransactionsQuery(string AccountNumber) : IRequest<IEnumerable<Transaction>>;
}
