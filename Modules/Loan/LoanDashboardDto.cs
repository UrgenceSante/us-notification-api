public class LoanDashBoardDto
{
    public required List<LoanComputedDto> LoanComputed { get; set; }
    public required List<LoanChartPoint> ChartPoints { get; set; }
}