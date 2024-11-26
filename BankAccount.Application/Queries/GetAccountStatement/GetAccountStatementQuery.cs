using BankAccount.Domain.Entities;
using MediatR;

namespace BankAccount.Application.Queries.GetAccountStatement
{
    public record GetAccountStatementQuery(string AccountNumber, DateTime Date) : IRequest<IEnumerable<Transaction>>;
}
