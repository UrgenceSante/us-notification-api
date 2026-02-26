namespace UsNotificationApi.Domain.Loans;

public class Loan
{
    public int Id { get; set; }
    public required string Label { get; set; }
    public string Category { get; set; } = "";
    public string Account { get; set; } = "";
    public required string Company { get; set; }
    public required string BankName { get; set; }
    public required double Capital { get; set; }
    public required double Rate { get; set; }
    public required int DurationInMonth { get; set; }
    public required DateTime StartDate { get; set; }


}