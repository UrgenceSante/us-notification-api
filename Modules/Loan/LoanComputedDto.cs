using PocMissionPush.Loan;


public class LoanComputedDto
{
    public required Loan Loan { get; set; }
    public double RemainingCapital { get; set; }
    public double Annuity { get; set; }
    public bool IsEnd { get; set; }
}