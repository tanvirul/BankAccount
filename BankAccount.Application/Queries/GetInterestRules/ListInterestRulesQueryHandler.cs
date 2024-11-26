using BankAccount.Domain.Entities;
using BankAccount.Domain.Repositories;
using MediatR;

namespace BankAccount.Application.Queries.GetInterestRules
{
    public class ListInterestRulesQueryHandler : IRequestHandler<ListInterestRulesQuery, List<InterestRule>>
    {
        private readonly IInterestRuleRepository _interestRuleRepository;

        public ListInterestRulesQueryHandler(IInterestRuleRepository interestRuleRepository)
        {
            _interestRuleRepository = interestRuleRepository;
        }

        public async Task<List<InterestRule>> Handle(ListInterestRulesQuery request, CancellationToken cancellationToken)
        {
            return await _interestRuleRepository.GetAllInterestRulesAsync();
        }
    }

}
