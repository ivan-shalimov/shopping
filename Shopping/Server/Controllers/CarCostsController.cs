using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopping.Server.Common;
using Shopping.Shared.Models.Results;
using Shopping.Shared.Requests;

namespace Shopping.Server.Controllers
{
    [Route("api/car-costs")]
    [ApiController]
    public class CarCostsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CarCostsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<CarCostModel[]>> GetCarCosts([FromQuery] int month)
        {
            var result = await _mediator.Send(new GetCarCosts { Month = month });
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddCarCost(AddCarCost request)
        {
            var result = await _mediator.Send(request);
            return result.Reduce();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCarCost(Guid id, UpdateCarCost request)
        {
            request.Id = id;
            var result = await _mediator.Send(request);
            return result.Reduce();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarCost(Guid id)
        {
            var result = await _mediator.Send(new DeleteCarCost { Id = id });
            return result.Reduce();
        }
    }
}