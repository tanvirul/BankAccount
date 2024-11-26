namespace BankAccount.Application.Services
{
    // IAccountService.cs
    public interface IAccountService
    {
        Task EnsureAccountExistsAsync(string accountNumber);
    }

}
