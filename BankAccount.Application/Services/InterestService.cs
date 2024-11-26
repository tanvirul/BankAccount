using BankAccount.Application.Commands.ApplyMonthlyInterest;
using MediatR;

namespace BankAccount.Application.Services
{
    public class InterestService : IInterestService
    {
        private readonly IMediator _mediator;
        public InterestService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task ApplyMonthlyInterest()
        {
            await _mediator.Send(new MonthlyInterestCommand());
        }
    }
}
