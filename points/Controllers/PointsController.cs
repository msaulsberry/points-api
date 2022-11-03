using System;
using Microsoft.AspNetCore.Mvc;
using points.Models;
using points.Models.Requests;
using points.Services.Interfaces;

namespace points.Controllers;

[ApiController]
[Route("[controller]")]
public class PointsController : ControllerBase
{
    private IPointService _pointService;

    public PointsController(IPointService pointService)
    {
        _pointService = pointService;
    }

    [HttpPost("transaction")]
    public IActionResult AddPointTransaction([FromBody] TransactionRequest request)
    {
        var success = _pointService.AddTransaction(request);

        // Could also return a 400 bad request here, but a 422 is a bit more accurate
        if (success == null)
            return new UnprocessableEntityResult();

        // Kept the response body simple since we don't know the requirements of the client
        return new CreatedResult(success, new { message = success});
    }

    [HttpPost("spend")]
    public ActionResult SpendPoints([FromBody] SpendEventRequest request)
    {
        var response = _pointService.SpendPoints(request);

        if (response == null)
            return new UnprocessableEntityResult();

        return new CreatedResult("success", response);
    }

    [HttpGet("payerBalances")]
    public ActionResult GetBalances()
    {
        object response = _pointService.GetPayerBalances();

        return new OkObjectResult(response);
    }
}

