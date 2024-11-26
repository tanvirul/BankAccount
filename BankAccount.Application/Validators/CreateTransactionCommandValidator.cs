using BankAccount.Application.Commands.CreateTransaction;
using FluentValidation;
using System.Text.RegularExpressions;

namespace BankAccount.Application.Validators
{
    public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
    {
        public CreateTransactionCommandValidator()
        {
            RuleFor(x => x.Date).Matches(@"^\d{8}$").WithMessage("Invalid date format. Please use YYYYMMdd.");
            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Amount must be greater than zero.")
                .Must(amount => decimal.Round(amount, 2) == amount).WithMessage("Amount can have up to 2 decimal places.");
            ;

            RuleFor(x => x.Type).Matches("^[DW]$", RegexOptions.IgnoreCase).WithMessage("Type must be 'D' (Deposit) or 'W' (Withdrawal).");
        }
    }

}
