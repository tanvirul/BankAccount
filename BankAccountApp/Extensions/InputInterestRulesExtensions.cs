using BankAccount.Application.Commands.InterestRule;
using System.Globalization;

namespace BankAccountApp.Extensions
{
    internal static class InputInterestRulesExtensions
    {
        public static DefineInterestRuleCommand ToDefineInterestRuleCommand(this string? input)
        {
            var parts = input?.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 3)
            {
                return null;
            }
            if (!DateTime.TryParseExact(parts[0], "yyyyMMdd", null, DateTimeStyles.None, out var date))
            {
                Console.WriteLine("Invalid date format.");
                return null;
            }
            if (!decimal.TryParse(parts[2], out var rate))
            {
                Console.WriteLine("Invalid rate format.");
                return null;
            }

            return new DefineInterestRuleCommand(date, RuleId: parts[1], rate);
        }
    }
}
