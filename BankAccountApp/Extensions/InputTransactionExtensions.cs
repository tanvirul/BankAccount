using BankAccount.Application.Commands.CreateTransaction;

namespace BankAccountApp.Extensions
{
    public static class InputTransactionExtensions
    {
        public static CreateTransactionCommand? ToCreateTransactionCommand(this string? input)
        {
            var details = input?.Split(' ');
            if (details == null || details.Length != 4)
            {
                return null;
            }
            return new CreateTransactionCommand(Date: details[0],
                AccountNumber: details[1],
                Type: details[2],
                Amount: decimal.Parse(details[3]));

        }
    }
}
