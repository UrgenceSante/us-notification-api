namespace UsNotificationApi.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using UsNotificationApi.Domain.Loans;

public class LoanDbContext : DbContext
{
    public LoanDbContext(DbContextOptions<LoanDbContext> options) : base(options)
    {

    }

    public DbSet<Loan> Loans { get; set; }
}