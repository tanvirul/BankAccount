namespace BankAccount.Application.Commands.Exceptions
{
    public class InsufficientFundException : Exception
    {
        public InsufficientFundException()
        {
        }
        public InsufficientFundException(string message) : base(message) { }
        public InsufficientFundException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
