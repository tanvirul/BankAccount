using BankAccount.Application.Commands.ApplyMonthlyInterest;
using BankAccount.Domain.Entities;
using BankAccount.Domain.Repositories;
using BankAccount.Domain.Services;
using Moq;

namespace BankAccount.Test.Application.Command
{
    public class MonthlyInterestCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidRequest_AppliesMonthlyInterest()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockAccountRepository = new Mock<IAccountRepository>();
            var mockTransactionRepository = new Mock<ITransactionRepository>();
            var mockInterestRuleRepository = new Mock<IInterestRuleRepository>();
            var mockInterestRuleApplyService = new Mock<IInterestRuleApplyService>();

            var accounts = new List<Account>
                {
                    new Account { AccountNumber = "12345", Balance = 50000 },
                    new Account { AccountNumber = "67890", Balance = 60000 }
                };

            var transactions = new List<Transaction>
            {
                new Transaction { Amount = 100m, Date = DateTime.UtcNow , BalanceAfterTransaction= 4500}
            };

            var interestRules = new List<InterestRule>
                {
                    new InterestRule { RuleId = "R001", Rate = 5.0m, Date = DateTime.UtcNow }
                };

            // Mock the repository methods
            mockUnitOfWork.Setup(u => u.AccountRepository.GetAccountsWithLockAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(accounts);

            mockUnitOfWork.Setup(u => u.TransactionRepository.GetMonthlyAccountStatements(It.IsAny<string>(), It.IsAny<DateTime>(), default))
                .ReturnsAsync(transactions);

            mockUnitOfWork.Setup(u => u.InterestRuleRepository.GetAllInterestRulesAsync())
                .ReturnsAsync(interestRules);

            mockInterestRuleApplyService.Setup(s => s.GetMonthlyInterestForTransactions(It.IsAny<List<Transaction>>(), It.IsAny<List<InterestRule>>()))
                .Returns(10.0m); // Returns the calculated interest for the account

            mockUnitOfWork.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            mockUnitOfWork.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);
            mockUnitOfWork.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);

            var handler = new MonthlyInterestCommandHandler(
                mockUnitOfWork.Object,
                mockInterestRuleApplyService.Object
            );

            var command = new MonthlyInterestCommand();

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            mockUnitOfWork.Verify(u => u.BeginTransactionAsync(), Times.AtLeastOnce);
            mockUnitOfWork.Verify(u => u.CommitAsync(), Times.AtLeastOnce);
            mockUnitOfWork.Verify(u => u.RollbackAsync(), Times.Never);
            mockUnitOfWork.Verify(u => u.AccountRepository.GetAccountsWithLockAsync(It.IsAny<int>(), It.IsAny<int>()), Times.AtLeastOnce);
            mockUnitOfWork.Verify(u => u.TransactionRepository.GetMonthlyAccountStatements(It.IsAny<string>(), It.IsAny<DateTime>(), default), Times.AtLeastOnce);
            mockUnitOfWork.Verify(u => u.InterestRuleRepository.GetAllInterestRulesAsync(), Times.Once);
            mockInterestRuleApplyService.Verify(s => s.GetMonthlyInterestForTransactions(It.IsAny<List<Transaction>>(), It.IsAny<List<InterestRule>>()), Times.AtLeastOnce);
            Assert.Equal(1, accounts[0].Transactions.Count); // Ensures a transaction is added
            Assert.Equal(1, accounts[1].Transactions.Count); // Ensures a transaction is added
        }

        [Fact]
        public async Task Handle_NoAccountsToProcess_DoesNotApplyInterest()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockAccountRepository = new Mock<IAccountRepository>();
            var mockTransactionRepository = new Mock<ITransactionRepository>();
            var mockInterestRuleRepository = new Mock<IInterestRuleRepository>();
            var mockInterestRuleApplyService = new Mock<IInterestRuleApplyService>();
            var interestRules = new List<InterestRule>
                {
                    new InterestRule { RuleId = "R001", Rate = 5.0m, Date = DateTime.UtcNow }
                };

            var accounts = new List<Account>(); // No accounts to process

            // Mock the repository methods
            mockUnitOfWork.Setup(u => u.AccountRepository.GetAccountsWithLockAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(accounts);

            mockUnitOfWork.Setup(u => u.TransactionRepository.GetMonthlyAccountStatements(It.IsAny<string>(), It.IsAny<DateTime>(), default))
                .ReturnsAsync(new List<Transaction>());

            mockUnitOfWork.Setup(u => u.InterestRuleRepository.GetAllInterestRulesAsync())
                .ReturnsAsync(interestRules);

            mockInterestRuleApplyService.Setup(s => s.GetMonthlyInterestForTransactions(It.IsAny<List<Transaction>>(), It.IsAny<List<InterestRule>>()))
                .Returns(10.0m); // Returns the calculated interest for the account

            mockUnitOfWork.Setup(u => u.AccountRepository.GetAccountsWithLockAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(accounts);

            var handler = new MonthlyInterestCommandHandler(
                mockUnitOfWork.Object,
                mockInterestRuleApplyService.Object
            );

            var command = new MonthlyInterestCommand();

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            mockUnitOfWork.Verify(u => u.AccountRepository.GetAccountsWithLockAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            mockUnitOfWork.Verify(u => u.TransactionRepository.GetMonthlyAccountStatements(It.IsAny<string>(), It.IsAny<DateTime>(), default), Times.Never);
            mockUnitOfWork.Verify(u => u.InterestRuleRepository.GetAllInterestRulesAsync(), Times.Once);
            mockInterestRuleApplyService.Verify(s => s.GetMonthlyInterestForTransactions(It.IsAny<List<Transaction>>(), It.IsAny<List<InterestRule>>()), Times.Never);
        }

        [Fact]
        public async Task Handle_FailsToGetAccounts_RollbacksTransaction()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockAccountRepository = new Mock<IAccountRepository>();
            var mockTransactionRepository = new Mock<ITransactionRepository>();
            var mockInterestRuleRepository = new Mock<IInterestRuleRepository>();
            var mockInterestRuleApplyService = new Mock<IInterestRuleApplyService>();

            mockUnitOfWork.Setup(u => u.AccountRepository.GetAccountsWithLockAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("Error fetching accounts"));

            var handler = new MonthlyInterestCommandHandler(
                mockUnitOfWork.Object,
                mockInterestRuleApplyService.Object
            );

            var command = new MonthlyInterestCommand();

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

            // Assert
            mockUnitOfWork.Verify(u => u.BeginTransactionAsync(), Times.Once);
            mockUnitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
            mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
        }

    }
}
