using BankAccount.Application.Commands.InterestRule;
using FluentValidation;

namespace BankAccount.Application.Validators
{
    public class DefineInterestRuleCommandValidator : AbstractValidator<DefineInterestRuleCommand>
    {
        public DefineInterestRuleCommandValidator()
        {
            RuleFor(x => x.Date).NotEmpty();
            RuleFor(x => x.RuleId).NotEmpty();
            RuleFor(x => x.Rate)
                .GreaterThan(0).WithMessage("Rate must be greater than 0.")
                .LessThan(100).WithMessage("Rate must be less than 100.");
        }
    }

}
