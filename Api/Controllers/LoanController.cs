using Microsoft.AspNetCore.Mvc;
using UsNotificationApi.Application.Loans;
using UsNotificationApi.Domain.Loans;

namespace UsNotificationApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoanController : ControllerBase
{
    private readonly ILoanService _loanService;

    public LoanController(ILoanService loanService)
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
