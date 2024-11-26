using AutoFixture;
using AutoFixture.AutoMoq;
using BankAccount.Test.Common.Factories.Entities;

namespace BankAccount.Test.Common.AutoFixture
{
    public static class FixtureExtensions
    {
        public static IFixture RegisterFactories(this IFixture fixture)
        {
            fixture.Behaviors
                   .OfType<ThrowingRecursionBehavior>()
                   .ToList()
                   .ForEach(b => fixture.Behaviors.Remove(b));

            fixture.Behaviors
                   .Add(new OmitOnRecursionBehavior(1));

            fixture.Customize(new AutoMoqCustomization());
            fixture.RegisterAccount();


            return fixture;
        }
    }
}
