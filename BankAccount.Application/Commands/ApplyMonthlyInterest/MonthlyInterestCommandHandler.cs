using BankAccount.Domain.Entities;
using BankAccount.Domain.Repositories;
using BankAccount.Domain.Services;
using MediatR;

namespace BankAccount.Application.Commands.ApplyMonthlyInterest
{
    public class MonthlyInterestCommandHandler : IRequestHandler<MonthlyInterestCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInterestRuleApplyService _interestRuleApplyService;
        public MonthlyInterestCommandHandler(IUnitOfWork unitOfWork, IInterestRuleApplyService interestRuleApplyService)
        {
            _unitOfWork = unitOfWork;
            _interestRuleApplyService = interestRuleApplyService;
        }
        public async Task<Unit> Handle(MonthlyInterestCommand request, CancellationToken cancellationToken)
        {
            int pageSize = 100;
            int pageNumber = 1;
            bool hasMoreData = true;
            var interestRules = await _unitOfWork.InterestRuleRepository.GetAllInterestRulesAsync();
            try
            {
                while (hasMoreData)
                {
                    await _unitOfWork.BeginTransactionAsync();
                    var accounts = await _unitOfWork.AccountRepository.GetAccountsWithLockAsync(pageSize, pageNumber);
                    if (accounts.Count() < pageSize)
                    {
                        hasMoreData = false;
                    }
                    foreach (var account in accounts)
                    {
                        List<decimal> calculatedInterests = new List<decimal>();
                        var transactions = await _unitOfWork.TransactionRepository.GetMonthlyAccountStatements(account.AccountNumber, DateTime.UtcNow);
                        var monthlyInterest = _interestRuleApplyService.GetMonthlyInterestForTransactions(transactions, interestRules);
                        account.AddTransaction(new Transaction
                        {
                            Date = DateTime.Today,
                            TxnId = string.Empty,
                            Type = "I",
                            Amount = monthlyInterest,
                            AccountNumber = account.AccountNumber
                        });
                        pageNumber++;
                    }
                    await _unitOfWork.CommitAsync();
                }

            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

            return Unit.Value;
        }
    }
}
