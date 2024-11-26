using BankAccount.Domain.Entities;
using Transaction = BankAccount.Domain.Entities.Transaction;

namespace BankAccount.Domain.Services
{
    public interface IInterestRuleApplyService
    {
        public decimal GetMonthlyInterestForTransactions(IEnumerable<Transaction> transactions, List<InterestRule> interestRules);
    }
    public class InterestRuleApplyService : IInterestRuleApplyService
    {
        public decimal GetMonthlyInterestForTransactions(IEnumerable<Transaction> transactions, List<InterestRule> interestRules)
        {
            if (transactions?.Any() != true || interestRules?.Any() != true)
                return 0;

            var orderedTransactions = transactions.OrderBy(x => x.Date.Date).ToList();
            var firstTransactionOfTheMonth = orderedTransactions.First().Date;

            var validInterestRules = FilterInterestRules(interestRules, firstTransactionOfTheMonth);

            DateTime startOfMonth = new DateTime(firstTransactionOfTheMonth.Year, firstTransactionOfTheMonth.Month, 1);
            DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            List<decimal> calculatedInterests = new List<decimal>();

            for (var currentDate = startOfMonth; currentDate <= endOfMonth; currentDate = currentDate.AddDays(1))
            {
                var interestRule = GetInterestRuleForDate(currentDate, validInterestRules);
                var eofTransaction = GetTransaction(currentDate, transactions);

                if (eofTransaction != null && interestRule != null)
                {
                    decimal interest = eofTransaction.BalanceAfterTransaction * (interestRule.Rate / 100);
                    calculatedInterests.Add(interest);
                }
            }

            return Math.Round(calculatedInterests.Sum() / 365, 2);
        }

        private Transaction GetTransaction(DateTime currentDate, IEnumerable<Transaction> transactions)
        {
            return transactions
                .Where(x => x.Date == currentDate || x.Date < currentDate)
                .OrderByDescending(x => x.Date)
                .FirstOrDefault();
        }

        private InterestRule GetInterestRuleForDate(DateTime currentDate, List<InterestRule> validInterestRules)
        {
            return validInterestRules
                .Where(x => x.Date <= currentDate)
                .OrderByDescending(x => x.Date)
                .FirstOrDefault();
        }

        private List<InterestRule> FilterInterestRules(List<InterestRule> interestRules, DateTime transactionMonth)
        {
            var rules = interestRules
                .Where(x => x.Date.Year == transactionMonth.Year && x.Date.Month == transactionMonth.Month)
                .ToList();

            if (!rules.Any())
            {
                var rule = interestRules
                    .Where(x => x.Date.Year <= transactionMonth.Year && x.Date.Month <= transactionMonth.Month)
                    .OrderByDescending(x => x.Date)
                    .FirstOrDefault();

                if (rule != null)
                {
                    rules.Add(rule);
                }
            }

            return rules;
        }
    }
}
