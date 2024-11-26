namespace BankAccount.Domain.Entities
{
    public class InterestRule
    {
        public int Id { get; set; } // Primary Key
        public DateTime Date { get; set; }
        public string RuleId { get; set; }
        public decimal Rate { get; set; }
    }

}
