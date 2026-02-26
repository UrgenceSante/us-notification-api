

namespace UsNotificationApi.Application.Loans;

using UsNotificationApi.Domain.Loans;

public class LoanDashboard
{
    public required Loan Loan { get; set; }
    public required LoanCalculatedInformation CalculatedInformation { get; set; }
}