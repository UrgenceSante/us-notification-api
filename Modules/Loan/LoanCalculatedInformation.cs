namespace PocMissionPush.LoanCalculatedInformation;

using PocMissionPush.Loan;


public class LoanCalculatedInformation : Loan
{
    public required double Annuity { get; set; }
    public required double RemainingCapital { get; set; }


}