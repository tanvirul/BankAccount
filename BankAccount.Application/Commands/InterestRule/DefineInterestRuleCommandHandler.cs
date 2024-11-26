using BankAccount.Domain.Repositories;
using MediatR;

namespace BankAccount.Application.Commands.InterestRule
{
    public class DefineInterestRuleCommandHandler : IRequestHandler<DefineInterestRuleCommand, Unit>
    {
        private readonly IInterestRuleRepository _interestRuleRepository;

        public DefineInterestRuleCommandHandler(IInterestRuleRepository interestRuleRepository)
        {
            _interestRuleRepository = interestRuleRepository;
        }

        public async Task<Unit> Handle(DefineInterestRuleCommand request, CancellationToken cancellationToken)
        {
            // Create or update the rule
            var interestRule = new BankAccount.Domain.Entities.InterestRule
            {
                Date = request.Date,
                RuleId = request.RuleId,
                Rate = request.Rate
            };

            await _interestRuleRepository.AddOrUpdateInterestRuleAsync(interestRule);

            return Unit.Value;
        }
    }

}
