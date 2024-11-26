using BankAccount.Application.Commands.InterestRule;
using BankAccount.Domain.Repositories;
using Moq;

namespace BankAccount.Test.Application.Command
{
    public class DefineInterestRuleCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidRequest_AddsOrUpdatesInterestRule()
        {
            // Arrange
            var mockInterestRuleRepository = new Mock<IInterestRuleRepository>();

            // Mock the AddOrUpdateInterestRuleAsync method to complete successfully
            mockInterestRuleRepository
                .Setup(r => r.AddOrUpdateInterestRuleAsync(It.IsAny<BankAccount.Domain.Entities.InterestRule>()))
                .Returns(Task.CompletedTask);

            var handler = new DefineInterestRuleCommandHandler(mockInterestRuleRepository.Object);

            var command = new DefineInterestRuleCommand(

                RuleId: "R001",
                Date: DateTime.Now,
                Rate: 5.5m
            );

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            mockInterestRuleRepository.Verify(r => r.AddOrUpdateInterestRuleAsync(It.Is<BankAccount.Domain.Entities.InterestRule>(ir =>
                ir.RuleId == command.RuleId &&
                ir.Rate == command.Rate &&
                ir.Date.Date == command.Date.Date)), Times.Once);
        }

        [Fact]
        public async Task Handle_InterestRuleAddFails_ThrowsException()
        {
            // Arrange
            var mockInterestRuleRepository = new Mock<IInterestRuleRepository>();

            // Mock AddOrUpdateInterestRuleAsync to throw an exception
            mockInterestRuleRepository
                .Setup(r => r.AddOrUpdateInterestRuleAsync(It.IsAny<BankAccount.Domain.Entities.InterestRule>()))
                .ThrowsAsync(new InvalidOperationException("Failed to add or update interest rule"));

            var handler = new DefineInterestRuleCommandHandler(mockInterestRuleRepository.Object);

            var command = new DefineInterestRuleCommand(

                 RuleId: "R001",
                 Date: DateTime.Now,
                 Rate: 5.5m
             );


            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));

            Assert.Equal("Failed to add or update interest rule", exception.Message);

            mockInterestRuleRepository.Verify(r => r.AddOrUpdateInterestRuleAsync(It.IsAny<BankAccount.Domain.Entities.InterestRule>()), Times.Once);
        }
    }
}
