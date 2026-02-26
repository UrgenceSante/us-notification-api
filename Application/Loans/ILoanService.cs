using UsNotificationApi.Domain.Loans;

namespace UsNotificationApi.Application.Loans;

public interface ILoanService
{
    List<Loan> GetLoans();
    Loan CreateLoan(Loan loan);
    LoanDashBoardDto GetLoanDashboard(DateTime? referenceDate, string? company, string? category);
}
