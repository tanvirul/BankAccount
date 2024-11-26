using BankAccount.Application.Services;
using BankAccount.Domain.Entities;
using BankAccount.Domain.Repositories;
using Moq;

namespace BankAccount.Test.Common
{
    public static class CommonMocks
    {
        public static Mock<IUnitOfWork> CreateMockUnitOfWork(
            out Mock<IAccountRepository> accountRepositoryMock,
            out Mock<ITransactionRepository> transactionRepositoryMock)
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            accountRepositoryMock = new Mock<IAccountRepository>();
            transactionRepositoryMock = new Mock<ITransactionRepository>();

            unitOfWorkMock.Setup(u => u.AccountRepository).Returns(accountRepositoryMock.Object);
            unitOfWorkMock.Setup(u => u.TransactionRepository).Returns(transactionRepositoryMock.Object);

            return unitOfWorkMock;
        }

        public static void SetupAccountRepository(Mock<IAccountRepository> accountRepositoryMock, Account account)
        {
            accountRepositoryMock
                .Setup(r => r.GetAccountWithLock(It.IsAny<string>()))
                .ReturnsAsync(account);
        }

        public static void SetupTransactionRepository(Mock<ITransactionRepository> transactionRepositoryMock, IEnumerable<Transaction> transactions)
        {
            transactionRepositoryMock
                .Setup(r => r.GetMonthlyAccountStatements(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(transactions);
        }

        public static Mock<IAccountService> CreateMockAccountService()
        {
            var accountServiceMock = new Mock<IAccountService>();
            accountServiceMock
                .Setup(s => s.EnsureAccountExistsAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            return accountServiceMock;
        }
    }
}
