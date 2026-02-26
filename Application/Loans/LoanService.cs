using UsNotificationApi.Domain.Loans;

namespace UsNotificationApi.Application.Loans;

public class LoanService : ILoanService
{
    private readonly ILoanRepository _loanRepository;

    public LoanService(ILoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
    }

    public List<Loan> GetLoans()
    {
        return _loanRepository.GetLoans();
    }

    public Loan CreateLoan(Loan loan)
    {
        return _loanRepository.CreateLoan(loan);
    }

    public List<LoanComputedDto> GetComputedLoans(DateTime? referenceDate, string? company, string? category)
    {
        List<Loan> loans = _loanRepository.GetLoans();

        return [.. loans
        .Where(l => !LoanTools.IsEnd(l, referenceDate ?? DateTime.Now) &&
         (string.IsNullOrEmpty(category) || l.Category == category) &&
          (string.IsNullOrEmpty(company) || l.Company == company))
        .Select(loan => new LoanComputedDto{
            Loan = loan,
            Annuity = LoanTools.GetAnnuity(loan),
            IsEnd = LoanTools.IsEnd(loan, referenceDate ?? DateTime.Now),
            RemainingCapital = LoanTools.GetRemainingCapital(loan, referenceDate ?? DateTime.Now),
        })];
    }

    public LoanDashBoardDto GetLoanDashboard(DateTime? referenceDate, string? company, string? category)
    {
        List<LoanComputedDto> computedLoans = [.. GetComputedLoans(referenceDate, company, category)];
        List<Loan> loans = [.. computedLoans.Select(l => l.Loan)];
        return new LoanDashBoardDto
        {
            LoanComputed = computedLoans,
            ChartPoints = [.. LoanTools.GetLoanChartPoints(loans, referenceDate ?? DateTime.Now)],
        };
    }
}
