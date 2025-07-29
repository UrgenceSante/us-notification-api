using Microsoft.AspNetCore.Mvc;
using PocMissionPush.Loan;

[ApiController]
[Route("api/[controller]")]
public class LoanController : ControllerBase
{
    public LoanService _loanService;

    public LoanController(LoanService loanService)
    {
        _loanService = loanService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_loanService.GetLoans());
    }

    [HttpGet("stats")]
    public IActionResult GetLoanStats([FromQuery] DateTime? referenceDate, [FromQuery] string? company, [FromQuery] string? category)
    {
        return Ok(_loanService.GetLoanDashboard(referenceDate, company, category));
    }

    [HttpPost]
    public IActionResult PostLoan([FromBody] Loan loan)
    {
        return Ok(_loanService.CreateLoan(loan));
    }




}