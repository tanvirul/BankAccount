using BankAccount.Application.Commands.ApplyMonthlyInterest;
using BankAccount.Application.Queries.GetAccountStatement;
using BankAccount.Application.Queries.GetInterestRules;
using BankAccount.Domain.Entities;
using BankAccountApp.Extensions;
using MediatR;

namespace BankAccountApp
{
    public class Menu
    {
        private IMediator mediator;
        public Menu(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public async Task ShowMenuAsync()
        {
            Console.WriteLine("Welcome to AwesomeGIC Bank! What would you like to do?");
            while (true)
            {
                Console.WriteLine("[T] Input transactions");
                Console.WriteLine("[I] Define interest rules");
                Console.WriteLine("[P] Print statement");
                Console.WriteLine("[Q] Quit");
                Console.Write("> ");
                var input = Console.ReadLine()?.Trim().ToUpper();

                if (input == "Q") break;

                switch (input)
                {
                    case "T":
                        Console.WriteLine("Enter transaction details in <Date> <Account> <Type> <Amount> format:");
                        var details = Console.ReadLine();
                        if (string.IsNullOrEmpty(details)) break;
                        var command = details.ToCreateTransactionCommand();
                        if (command is null)
                        {
                            Console.WriteLine("Invalid input");
                            break;
                        }
                        await mediator.Send(command);
                        await PrintAccountTransactions(command.AccountNumber);
                        break;

                    case "I":
                        Console.WriteLine("Please enter interest rules details in <Date> <RuleId> <Rate in %> format (or enter blank to go back to main menu):");
                        var interestRules = Console.ReadLine();
                        if (string.IsNullOrEmpty(interestRules)) break;
                        var interestRulecommand = interestRules.ToDefineInterestRuleCommand();
                        await mediator.Send(interestRulecommand);
                        await PrintInterestRules();
                        break;
                    case "P":
                        Console.WriteLine("Please enter account and month to generate the statement <Account> <Year><Month>\r\n(or enter blank to go back to main menu):");
                        var statementInput = Console.ReadLine();
                        if (string.IsNullOrEmpty(statementInput)) break;
                        var query = statementInput.ToAccountStatementQuery();
                        var result = await mediator.Send(query);
                        PrintStatements(result, query.AccountNumber);
                        break;
                    case "M":
                        await mediator.Send(new MonthlyInterestCommand());
                        break;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
                Console.WriteLine("Is there anything else you'd like to do?");
            }
        }

        private void PrintStatements(IEnumerable<Transaction> transactions, string AccountNumber)
        {
            Console.WriteLine($"Account: {AccountNumber}");
            foreach (var txn in transactions)
            {
                Console.WriteLine($"{txn.Date:yyyyMMdd} | {txn.TxnId} | {txn.Type} | {txn.Amount:F2} | {txn.BalanceAfterTransaction}");
            }
        }

        private async Task PrintInterestRules()
        {
            var rules = await mediator.Send(new ListInterestRulesQuery());
            Console.WriteLine("Interest rules:");
            Console.WriteLine("| Date     | RuleId | Rate (%) |");
            foreach (var rule in rules)
            {
                Console.WriteLine($"| {rule.Date:yyyyMMdd} | {rule.RuleId} | {rule.Rate,8:F2} |");
            }
        }

        private async Task PrintAccountTransactions(string? accountNumber)
        {
            var transactions = await mediator.Send(new GetAccountTransactionsQuery(accountNumber));

            Console.WriteLine($"Account: {accountNumber}");
            foreach (var txn in transactions)
            {
                Console.WriteLine($"{txn.Date:yyyyMMdd} | {txn.TxnId} | {txn.Type} | {txn.Amount:F2}");
            }
        }
    }

}