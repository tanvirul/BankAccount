using BankAccount.Domain.Entities;
using BankAccount.Domain.Repositories;
using BankAccount.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BankAccount.Infrastructure.Repositories
{
    public class InterestRuleRepository : IInterestRuleRepository
    {
        private readonly AppDbContext _dbContext;

        public InterestRuleRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddOrUpdateInterestRuleAsync(InterestRule interestRule)
        {
            // Check if a rule exists for the same date
            var existingRule = await _dbContext.InterestRules
                .Where(r => r.Date == interestRule.Date)
                .FirstOrDefaultAsync();

            if (existingRule != null)
            {
                // Update existing rule
                existingRule.RuleId = interestRule.RuleId;
                existingRule.Rate = interestRule.Rate;
            }
            else
            {
                // Add new rule
                await _dbContext.InterestRules.AddAsync(interestRule);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<InterestRule>> GetAllInterestRulesAsync()
        {
            return await _dbContext.InterestRules.AsNoTracking()
                .OrderBy(r => r.Date)
                .ToListAsync();
        }
    }

}
