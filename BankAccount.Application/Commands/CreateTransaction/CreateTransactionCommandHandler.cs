using BankAccount.Application.Services;
using BankAccount.Domain.Entities;
using BankAccount.Domain.Repositories;
using MediatR;
using System.Globalization;

namespace BankAccount.Application.Commands.CreateTransaction
{
    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountService _accountService;

        public CreateTransactionCommandHandler(IUnitOfWork unitOfWork, IAccountService accountService)
        {
            _unitOfWork = unitOfWork;
            _accountService = accountService;
        }

        public async Task<Unit> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            DateTime.TryParseExact(request.Date, "yyyyMMdd", null, DateTimeStyles.None, out var date);
            await _accountService.EnsureAccountExistsAsync(request.AccountNumber);
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var account = await _unitOfWork.AccountRepository.GetAccountWithLock(request.AccountNumber);
                var transactions = await _unitOfWork.TransactionRepository.GetMonthlyAccountStatements(request.AccountNumber, date, cancellationToken);

                if (account == null)
                {
                    throw new InvalidOperationException("Account does not exist.");
                }

                string txnId = $"{request.Date}-{transactions.Count() + 1:D2}";

                var transaction = new Transaction
                {
                    Date = date,
                    TxnId = txnId,
                    Type = request.Type,
                    Amount = request.Amount,
                    AccountNumber = request.AccountNumber
                };

                account.AddTransaction(transaction);
                await _unitOfWork.CommitAsync();
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
