using BankAccount.Domain.Entities;
using MediatR;

namespace BankAccount.Application.Queries.GetAccount
{
    public record GetAccountQuery(string AccountNumber) : IRequest<Account>;

}
