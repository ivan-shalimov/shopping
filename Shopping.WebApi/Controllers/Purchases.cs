using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopping.Models.Results;
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

        [HttpGet]
        public async Task<ActionResult<PurchaseItem[]>> GetPurchases()
        {
            var result = await _mediator.Send(new GetPurchases());
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddPurchases(AddPurchase request)
        {
            await _mediator.Send(request);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchases(Guid id)
        {
            await _mediator.Send(new DeletePurchase { Id = id });
            return Ok();
        }

        [HttpGet("statistic")]
        public async Task<ActionResult<PurchaseStatistic>> GetPurchaseStatistic()
        {
            var result = await _mediator.Send(new GetPurchaseStatistic());
            return Ok(result);
        }
    }
}