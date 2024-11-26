using BankAccount.Domain.Entities;
using BankAccount.Domain.Services;

namespace BankAccount.Test.Domain
{
    public class InterestRuleApplyServiceTests
    {
        private readonly InterestRuleApplyService _service;

        public InterestRuleApplyServiceTests()
        {
            _service = new InterestRuleApplyService();
        }

        [Fact]
        public void GetMonthlyInterestForTransactions_WithValidData_ReturnsCorrectInterest()
        {
            // Arrange
            var transactions = new List<Transaction>
        {
            new Transaction { Date = new DateTime(2023, 6, 1), BalanceAfterTransaction = 250m },
            new Transaction { Date = new DateTime(2023, 6, 14), BalanceAfterTransaction = 250m },
            new Transaction { Date = new DateTime(2023, 6, 15), BalanceAfterTransaction = 250m },
            new Transaction { Date = new DateTime(2023, 6, 25), BalanceAfterTransaction = 250m },
            new Transaction { Date = new DateTime(2023, 6, 26), BalanceAfterTransaction = 130m },
            new Transaction { Date = new DateTime(2023, 6, 30), BalanceAfterTransaction = 130m }
        };

            var interestRules = new List<InterestRule>
        {
            new InterestRule { RuleId = "RULE02", Rate = 1.90m, Date = new DateTime(2023, 6, 1) },
            new InterestRule { RuleId = "RULE03", Rate = 2.20m, Date = new DateTime(2023, 6, 15) }
        };

            // Expected Interest Calculation:
            // Period 1 (2023-06-01 to 2023-06-14): 250 * 1.90% * 14 = 66.5m
            // Period 2 (2023-06-15 to 2023-06-25): 250 * 2.20% * 11 = 60.50M
            // Period 3 (2023-06-26 to 2023-06-30): 130 * 2.20% * 5 = 14.30M 
            var expectedInterest = Math.Round((66.50m + 60.50m + 14.30m) / 365, 2);
            // Act
            var result = _service.GetMonthlyInterestForTransactions(transactions, interestRules);

            // Assert
            Assert.Equal(expectedInterest, result);
        }

        [Fact]
        public void GetMonthlyInterestForTransactions_WithEmptyTransactions_ReturnsZero()
        {
            // Arrange
            var transactions = new List<Transaction>();
            var interestRules = new List<InterestRule>
        {
            new InterestRule { RuleId = "RULE02", Rate = 1.90m, Date = new DateTime(2023, 6, 1) }
        };

            // Act
            var result = _service.GetMonthlyInterestForTransactions(transactions, interestRules);

            // Assert
            Assert.Equal(0m, result);
        }

        [Fact]
        public void GetMonthlyInterestForTransactions_WithNoInterestRules_ReturnsZero()
        {
            // Arrange
            var transactions = new List<Transaction>
        {
            new Transaction { Date = new DateTime(2023, 6, 1), BalanceAfterTransaction = 250m }
        };
            var interestRules = new List<InterestRule>();

            // Act
            var result = _service.GetMonthlyInterestForTransactions(transactions, interestRules);

            // Assert
            Assert.Equal(0m, result);
        }

        [Fact]
        public void GetMonthlyInterestForTransactions_WithSingleInterestRuleAndFutureDate_ReturnsCorrectInterest()
        {
            // Arrange
            var transactions = new List<Transaction>
        {
            new Transaction { Date = new DateTime(2023, 6, 1), BalanceAfterTransaction = 250m }
        };

            var interestRules = new List<InterestRule>
        {
            new InterestRule { RuleId = "RULE02", Rate = 1.90m, Date = new DateTime(2023, 6, 1) }
        };

            // Expected Interest Calculation:
            // Period (2023-06-01 to end of the month): 250 * 1.90% * DaysFrom20230601
            var expectedInterest = Math.Round((250 * 0.019m * 30) / 365, 2);

            // Act
            var result = _service.GetMonthlyInterestForTransactions(transactions, interestRules);

            // Assert
            Assert.Equal(expectedInterest, result);
        }
    }

}
