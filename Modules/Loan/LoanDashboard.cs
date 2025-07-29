

namespace PocMissionPush.LoanDashboard;

using PocMissionPush.Loan;
using PocMissionPush.LoanCalculatedInformation;

public class LoanDashboard
{
    public required Loan Loan { get; set; }
    public required LoanCalculatedInformation CalculatedInformation { get; set; }
}