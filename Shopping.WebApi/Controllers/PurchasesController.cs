using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopping.Models.Results;
using Shopping.Requests;

namespace Shopping.WebApi.Controllers
{
    [Route("api/purchases")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PurchasesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("statistic")]
        public async Task<ActionResult<PurchaseStatistic>> GetPurchaseStatistic([FromQuery]int month)
        {
            var result = await _mediator.Send(new GetPurchaseStatistic { Month = month });
            return Ok(result);
        }
    }
}