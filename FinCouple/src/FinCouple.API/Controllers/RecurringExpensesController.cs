using FinCouple.Application.DTOs.RecurringExpenses;
using FinCouple.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinCouple.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RecurringExpensesController : ControllerBase
{
    private readonly IRecurringExpenseService _service;

    public RecurringExpensesController(IRecurringExpenseService service)
    {
        _service = service;
    }

    [HttpGet("couple/{coupleId:guid}")]
    public async Task<IActionResult> GetByCouple(Guid coupleId, CancellationToken cancellationToken)
    {
        var result = await _service.GetByCoupleAsync(coupleId, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRecurringExpenseRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetByCouple), new { coupleId = result.CoupleId }, result);
    }

    [HttpPatch("{id:guid}/toggle")]
    public async Task<IActionResult> ToggleActive(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.ToggleActiveAsync(id, cancellationToken);
        if (result is null) return NotFound();
        return Ok(result);
    }
}
