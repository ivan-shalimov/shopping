using Microsoft.AspNetCore.Mvc;
using Shopping.Mediator;
using Shopping.Shared.Models.Results;
using Shopping.Shared.Requests.Prices;

namespace Shopping.Server.Controllers
{
    [Route("api/prices")]
    [ApiController]
    public class PricesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PricesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("latest")]
        public async Task<ActionResult<PurchaseStatistic>> GetLastProductsPrices([FromQuery] Guid receiptId, [FromQuery] Guid[] productIds)
        {
            var result = await _mediator.ExecuteAndReceiveWithoutValidation<GetLastProductsPrices, IDictionary<Guid, decimal>>(new GetLastProductsPrices { ReceiptId = receiptId, ProductIds = productIds });
            return Ok(result);
        }
    }
}