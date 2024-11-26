using BankAccount.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankAccount.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<InterestRule> InterestRules { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().ToTable(nameof(Account));
            modelBuilder.Entity<Transaction>().ToTable(nameof(Transaction));
            modelBuilder.Entity<InterestRule>().ToTable(nameof(InterestRule));

            modelBuilder.Entity<Account>()
                .HasKey(a => a.AccountNumber);

            modelBuilder.Entity<Account>()
               .Property(t => t.Balance)
               .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<InterestRule>()
                .HasKey(a => a.Id);


            modelBuilder.Entity<Transaction>()
                .HasKey(t => t.Id);
            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Transaction>()
                .Property(t => t.BalanceAfterTransaction)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Transaction>()
                      .HasOne(t => t.Account)
                      .WithMany(t => t.Transactions)
                      .HasForeignKey(t => t.AccountNumber);
        }
    }

}
