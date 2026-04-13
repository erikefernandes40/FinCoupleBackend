using FinCouple.Application.DTOs.Budgets;
using FinCouple.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinCouple.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BudgetsController : ControllerBase
{
    private readonly IBudgetService _budgetService;

    public BudgetsController(IBudgetService budgetService)
    {
        _budgetService = budgetService;
    }

    [HttpGet("couple/{coupleId:guid}")]
    public async Task<IActionResult> GetByCoupleAndMonth(Guid coupleId, [FromQuery] int month, [FromQuery] int year, CancellationToken cancellationToken)
    {
        var result = await _budgetService.GetByCoupleAndMonthAsync(coupleId, month, year, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Upsert([FromBody] UpsertBudgetRequest request, CancellationToken cancellationToken)
    {
        var result = await _budgetService.UpsertAsync(request, cancellationToken);
        return Ok(result);
    }
}
