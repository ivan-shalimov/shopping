using Microsoft.AspNetCore.Mvc;
using Shopping.Mediator;
using Shopping.Shared.Models.Results;
using Shopping.Shared.Requests;

namespace Shopping.Server.Controllers
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
        public async Task<ActionResult<PurchaseStatistic>> GetPurchaseStatistic([FromQuery] int month)
        {
            var result = await _mediator.ExecuteAndReceiveWithoutValidation<GetPurchaseStatistic, PurchaseStatistic>(new GetPurchaseStatistic { Month = month });
            return Ok(result);
        }
    }
}