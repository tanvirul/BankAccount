using BankAccount.Application.Commands.CreateAccount;
using BankAccount.Domain.Entities;
using BankAccount.Domain.Repositories;
using Moq;

namespace BankAccount.Test.Application.Command
{
    public class CreateAccountCommandHandlerTests
    {
        [Fact]
        public async Task Handle_AccountDoesNotExist_CreatesNewAccount()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockAccountRepository = new Mock<IAccountRepository>();

            mockUnitOfWork.Setup(u => u.AccountRepository).Returns(mockAccountRepository.Object);

            mockAccountRepository.Setup(r => r.GetAccountAsync(It.IsAny<string>()))
                .ReturnsAsync((Account)null);

            var handler = new CreateAccountCommandHandler(mockUnitOfWork.Object);

            var command = new CreateAccountCommand(AccountNumber: "12345");


            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            mockAccountRepository.Verify(r => r.GetAccountAsync(command.AccountNumber), Times.Once);
            mockAccountRepository.Verify(r => r.AddAccountAsync(It.Is<Account>(a => a.AccountNumber == command.AccountNumber)), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_AccountAlreadyExists_ThrowsException()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockAccountRepository = new Mock<IAccountRepository>();

            mockUnitOfWork.Setup(u => u.AccountRepository).Returns(mockAccountRepository.Object);

            // Ensure GetAccountAsync returns an existing account (i.e., the account exists)
            mockAccountRepository.Setup(r => r.GetAccountAsync(It.IsAny<string>()))
                .ReturnsAsync(new Account { AccountNumber = "12345" });

            var handler = new CreateAccountCommandHandler(mockUnitOfWork.Object);

            var command = new CreateAccountCommand(AccountNumber: "12345");

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));

            Assert.Equal("Account 12345 already exists.", exception.Message);

            mockAccountRepository.Verify(r => r.GetAccountAsync(command.AccountNumber), Times.Once);
            mockAccountRepository.Verify(r => r.AddAccountAsync(It.IsAny<Account>()), Times.Never);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Never);
        }
    }
}
