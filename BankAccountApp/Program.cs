using BankAccount.Application.Services;
using BankAccount.Application.Validators;
using BankAccount.Domain.Repositories;
using BankAccount.Domain.Services;
using BankAccount.Infrastructure.Persistence;
using BankAccount.Infrastructure.Repositories;
using BankAccountApp.Jobs;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;

namespace BankAccountApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory()) // Current directory of the app
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .Build();

            // Retrieve the connection string
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var monthlyInterestJobSchedule = configuration.GetValue<string>("MonthlyInterestJobSchedule");
            var host = new HostBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("BankAccount.Infrastructure")));
                           //.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information));
                services.AddScoped<IAccountRepository, AccountRepository>();
                services.AddScoped<IInterestRuleRepository, InterestRuleRepository>();
                services.AddScoped<ITransactionRepository, TransactionRepository>();
                services.AddScoped<IUnitOfWork, UnitOfWork>();

                services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(BankAccount.Application.Commands.CreateTransaction.CreateTransactionCommand).Assembly));
                services.AddValidatorsFromAssemblyContaining<CreateTransactionCommandValidator>();
                services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

                services.AddScoped<IAccountService, AccountService>();
                services.AddScoped<IInterestService, InterestService>();
                services.AddScoped<IInterestRuleApplyService, InterestRuleApplyService>();


                services.AddTransient<Menu>();
                services.AddQuartz(q =>
                {
                    q.UseMicrosoftDependencyInjectionJobFactory();
                    var jobKey = new JobKey("MonthlyJob");
                    q.AddJob<InterestApplyMonthlyJob>(opts => opts.WithIdentity(jobKey));

                    q.AddTrigger(opts => opts
                        .ForJob(jobKey) // Link trigger to job
                        .WithIdentity("LastDayTrigger")
                        .WithCronSchedule(monthlyInterestJobSchedule)); // Cron for last day of the month

                });
                // Quartz.Extensions.Hosting hosting
                services.AddQuartzHostedService(options =>
                {
                    options.WaitForJobsToComplete = true;
                });

            })
            .Build();
            using (var scope = host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.Migrate(); // Apply migrations
            }
           
            var menu = host.Services.GetRequiredService<Menu>();
            await menu.ShowMenuAsync();
        }
    }
}