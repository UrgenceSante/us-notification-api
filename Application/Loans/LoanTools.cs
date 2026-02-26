namespace UsNotificationApi.Application.Loans;

using UsNotificationApi.Domain.Loans;

public static class LoanTools
{

    public static double GetAnnuity(Loan loan)
    {

        double monthlyRate = loan.Rate / 12;
        if (monthlyRate == 0) return loan.Capital / loan.DurationInMonth;
        return loan.Capital * monthlyRate / (1 - Math.Pow(1 + monthlyRate, -loan.DurationInMonth));

    }

    public static bool IsEnd(Loan loan, DateTime referenceDate)
    {
        int elapsedMonths = (referenceDate.Year - loan.StartDate.Year) * 12 + referenceDate.Month - loan.StartDate.Month;
        if (elapsedMonths <= loan.DurationInMonth) return false;
        else return true;
    }


    public static double GetRemainingCapital(Loan loan, DateTime date)
    {
        double capital = (double)loan.Capital;
        double monthlyRate = (double)loan.Rate / 12 / 100;
        int totalMonths = loan.DurationInMonth;
        int elapsedMonths = (date.Year - loan.StartDate.Year) * 12 + date.Month - loan.StartDate.Month;
        double annuity = GetAnnuity(loan);


        if (elapsedMonths <= 0) return capital;
        if (elapsedMonths >= totalMonths) return 0;

        if (monthlyRate == 0) return capital * (1 - (elapsedMonths / totalMonths));
        else
            return annuity * (1 - Math.Pow(1 + monthlyRate, -(totalMonths - elapsedMonths))) / monthlyRate;
    }

    public static double GetTotalRemainingCapital(List<Loan> loans, DateTime date)
    {
        return loans.Sum(loan => LoanTools.GetRemainingCapital(loan, date));
    }

    public static List<LoanChartPoint> GetLoanChartPoints(List<Loan> loans, DateTime referenceDate)
    {

        List<DateTime> monthlyDates = [.. Enumerable.Range(0, 120).Select(i => new DateTime(referenceDate.Year, referenceDate.Month, 1).AddMonths(i))];

        return [.. monthlyDates.Select(d => new LoanChartPoint{
                X = d,
                Y = GetTotalRemainingCapital(loans, d)
            })];
    }

    public static decimal PowDecimal(decimal value, int exponent)
    {
        if (exponent == 0) return 1;
        if (exponent < 0) return 1 / PowDecimal(value, -exponent);

        decimal result = 1;
        for (int i = 0; i < exponent; i++)
        {
            result *= value;
        }

        return result;
    }


}