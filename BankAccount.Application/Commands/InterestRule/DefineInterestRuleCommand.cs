using MediatR;

namespace BankAccount.Application.Commands.InterestRule
{
    public record DefineInterestRuleCommand(DateTime Date, string RuleId, decimal Rate) : IRequest<Unit>;
}
