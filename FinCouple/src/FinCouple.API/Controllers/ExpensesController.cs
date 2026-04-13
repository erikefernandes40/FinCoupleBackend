using FinCouple.Application.DTOs.Expenses;
using FinCouple.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinCouple.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExpensesController : ControllerBase
{
    private readonly IExpenseService _expenseService;

    public ExpensesController(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    [HttpGet("couple/{coupleId:guid}")]
    public async Task<IActionResult> GetByCouple(Guid coupleId, CancellationToken cancellationToken)
    {
        var result = await _expenseService.GetByCoupleAsync(coupleId, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateExpenseRequest request, CancellationToken cancellationToken)
    {
        var result = await _expenseService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetByCouple), new { coupleId = result.CoupleId }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateExpenseRequest request, CancellationToken cancellationToken)
    {
        var result = await _expenseService.UpdateAsync(id, request, cancellationToken);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _expenseService.DeleteAsync(id, cancellationToken);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
