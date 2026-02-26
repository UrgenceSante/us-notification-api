namespace UsNotificationApi.Infrastructure.Persistence;

using UsNotificationApi.Domain.Loans;

public class LoanRepository : ILoanRepository
{
    private readonly LoanDbContext _context;

    public LoanRepository(LoanDbContext context)
    {
        _context = context;
    }

    public Loan CreateLoan(Loan loan)
    {
        _context.Loans.Add(loan);
        _context.SaveChanges();
        return loan;
    }

    public Loan DeleteLoan(int loanId)
    {
        throw new NotImplementedException();
    }

    public Loan GetLoanById(int loanId)
    {
        throw new NotImplementedException();
    }

    public List<Loan> GetLoans()
    {
        return [.. _context.Loans];
    }

    public Loan UpdateLoan(Loan loan)
    {
        throw new NotImplementedException();
    }
}
