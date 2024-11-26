using BankAccount.Application.Services;
using Quartz;

namespace BankAccountApp.Jobs
{
    public class InterestApplyMonthlyJob : IJob
    {
        private IInterestService _interestService;
        public InterestApplyMonthlyJob(IInterestService interestService)
        {
            _interestService = interestService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await _interestService.ApplyMonthlyInterest();
        }
    }
}
