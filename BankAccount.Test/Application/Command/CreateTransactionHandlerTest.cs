using BankAccount.Application.Commands.CreateTransaction;
using BankAccount.Domain.Entities;
using BankAccount.Test.Common;
using Moq;

namespace BankAccount.Test.Application.Command
{
    public class CreateTransactionHandlerTest
    {
        [Fact]
        public async Task Handle_ValidRequest_CreatesTransactionAndCommits()
        {
            // Arrange
            var unitOfWorkMock = CommonMocks.CreateMockUnitOfWork(
                out var accountRepositoryMock,
                out var transactionRepositoryMock);

            var account = new Account { AccountNumber = "12345" };
            var transactions = Enumerable.Empty<Transaction>();

            CommonMocks.SetupAccountRepository(accountRepositoryMock, account);
            CommonMocks.SetupTransactionRepository(transactionRepositoryMock, transactions);

            var accountServiceMock = CommonMocks.CreateMockAccountService();
            var handler = new CreateTransactionCommandHandler(unitOfWorkMock.Object, accountServiceMock.Object);

            var command = new CreateTransactionCommand(

                AccountNumber: "12345",
                Date: "20241101",
                Type: "D",
                Amount: 100.00m
            );

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            accountServiceMock.Verify(s => s.EnsureAccountExistsAsync(command.AccountNumber), Times.Once);
            unitOfWorkMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
            unitOfWorkMock.Verify(u => u.RollbackAsync(), Times.Never);
            accountRepositoryMock.Verify(r => r.GetAccountWithLock(command.AccountNumber), Times.Once);
            transactionRepositoryMock.Verify(r => r.GetMonthlyAccountStatements(command.AccountNumber, It.IsAny<DateTime>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.Single(account.Transactions);
        }
    }
}
