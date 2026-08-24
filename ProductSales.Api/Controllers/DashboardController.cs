using Microsoft.AspNetCore.Mvc;
using ProductSales.Application.DTOs;
using ProductSales.Application.Services;
namespace ProductSales.Api.Controllers;
[ApiController][Route("api/dashboard")]
public class DashboardController(IProductDashboardService service):ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(DashboardDto), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 502)]
    public async Task<ActionResult<DashboardDto>> Get(CancellationToken ct)
    {
        return Ok(await service.GetAsync(ct));
    }
}
