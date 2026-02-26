
namespace UsNotificationApi.Domain.Loans;
public interface ILoanRepository
{
    Loan CreateLoan(Loan loan);
    List<Loan> GetLoans();
    Loan GetLoanById(int loanId);
    Loan UpdateLoan(Loan loan);
    Loan DeleteLoan(int loanId);

}