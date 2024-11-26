using BankAccount.Application.Queries.GetAccountStatement;
using System.Globalization;

namespace BankAccountApp.Extensions
{
    public static class InputAccountStatementExtensions
    {
        public static GetAccountStatementQuery ToAccountStatementQuery(this string? input)
        {
            var parts = input?.Split(' ');
            if (parts == null || parts.Length != 2)
            {
                return null;
            }
            if (!DateTime.TryParseExact(parts[1], "yyyyMM", null, DateTimeStyles.None, out var date))
            {
                Console.WriteLine("Invalid date format.");
                return null;
            }
            return new GetAccountStatementQuery(AccountNumber: parts[0], date);
        }
    }
}
