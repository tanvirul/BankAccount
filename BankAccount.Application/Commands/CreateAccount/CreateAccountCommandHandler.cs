using BankAccount.Domain.Entities;
using BankAccount.Domain.Repositories;
using MediatR;

namespace BankAccount.Application.Commands.CreateAccount
{
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateAccountCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            // Ensure account does not already exist
            var existingAccount = await _unitOfWork.AccountRepository.GetAccountAsync(request.AccountNumber);
            if (existingAccount != null)
            {
                throw new InvalidOperationException($"Account {request.AccountNumber} already exists.");
            }

            // Create a new account
            var newAccount = new Account
            {
                AccountNumber = request.AccountNumber,
                Balance = 0
            };

            await _unitOfWork.AccountRepository.AddAccountAsync(newAccount);
            await _unitOfWork.SaveAsync();

            return Unit.Value;
        }
    }
}
