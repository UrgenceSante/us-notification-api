namespace UsNotificationApi.Application.Loans;

using UsNotificationApi.Domain.Loans;


public class LoanCalculatedInformation : Loan
{
    public required double Annuity { get; set; }
    public required double RemainingCapital { get; set; }


}