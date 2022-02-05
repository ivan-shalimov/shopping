using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopping.Models;
using Shopping.Requests;

namespace Shopping.WebApi.Controllers
{
    [Route("api/purchases")]
    [ApiController]
    public class Purchases : ControllerBase
    {
        private readonly IMediator _mediator;

        public Purchases(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("statistic")]
        public async Task<ActionResult<PurchaseStatistic>> GetPurchaseStatistic()
        {
            var result = await _mediator.Send(new GetPurchaseStatistic());
            return Ok(result);
        }
    }
}