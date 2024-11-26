namespace BankAccount.Application.Commands.Exceptions
{
    public class NegativeBalanceException : Exception
    {
        public NegativeBalanceException()
        {
        }
        public NegativeBalanceException(string message) : base(message) { }
        public NegativeBalanceException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
