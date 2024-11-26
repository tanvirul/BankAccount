using BankAccount.Domain.Entities;
using MediatR;

namespace BankAccount.Application.Queries.GetInterestRules
{
    public class ListInterestRulesQuery : IRequest<List<InterestRule>>
    {
    }
}
