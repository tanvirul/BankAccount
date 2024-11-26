using BankAccount.Domain.Entities;

namespace BankAccount.Domain.Repositories
{
    public interface IInterestRuleRepository
    {
        Task AddOrUpdateInterestRuleAsync(InterestRule interestRule);
        Task<List<InterestRule>> GetAllInterestRulesAsync();
    }

}
