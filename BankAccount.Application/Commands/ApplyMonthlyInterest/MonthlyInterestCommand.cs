using MediatR;

namespace BankAccount.Application.Commands.ApplyMonthlyInterest
{
    public class MonthlyInterestCommand : IRequest<Unit>
    {
    }
}
