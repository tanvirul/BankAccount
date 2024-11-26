using AutoFixture;
using AutoFixture.Xunit2;

namespace BankAccount.Test.Common.AutoFixture
{
    public class AnonymousDataAttribute : AutoDataAttribute
    {
        private static readonly Func<IFixture> fixture = () => new Fixture().RegisterFactories();

        public AnonymousDataAttribute() : base(fixture)
        {
        }
    }
}
