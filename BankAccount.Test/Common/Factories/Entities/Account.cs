using AutoFixture;
using BankAccount.Domain.Entities;
using Moq;

namespace BankAccount.Test.Common.Factories.Entities
{
    public class AccountFactory
    {
        public static Account Create()
        {
            return new Fixture().RegisterAccountMock()
                                .Create<Account>();
        }

        public static IEnumerable<Account> CreateMany()
        {
            return new Fixture().RegisterAccountMock()
                                .CreateMany<Account>();
        }
    }

    public static class AccountFactoryExtension
    {
        public static IFixture RegisterAccount(this IFixture fixture)
        {
            fixture.Register(() => new Account
            {
                AccountNumber = fixture.Create<string>(),
                Balance = fixture.Create<decimal>()
            });
            return fixture;
        }

        public static IFixture RegisterAccountMock(this IFixture fixture)
        {
            fixture.Register(() =>
            {
                var mock = new Mock<Account>(fixture.Create<string>(),
                                               fixture.Create<decimal>());
                mock.SetupGet(x => x.AccountNumber).Returns(fixture.Create<string>());
                return mock.Object;
            });
            return fixture;
        }
    }
}
