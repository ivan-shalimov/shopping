using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopping.Shared.Models.Results;
using Shopping.Shared.Requests.Statistic;

namespace Shopping.Server.Controllers
{
    [Route("api/statistic")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StatisticController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("expenses-by-kind/{month}/month")]
        public async Task<ActionResult<PurchaseStatistic>> GetExpensesByKind([FromRoute] string month)
        {
            var now = DateTime.UtcNow;
            var startOfMonth = month switch
            {
                "current" => new DateTime(now.Year, now.Month, 1),
                "previous" => new DateTime(now.Year, now.Month, 1).AddMonths(-1),
            };
            var result = await _mediator.Send(new GetExpensesByKind { StartOfMonth = startOfMonth });
            return Ok(result);
        }

        [HttpGet("expenses-by-month/{year}/year")]
        public async Task<ActionResult<PurchaseStatistic>> GetExpensesByMonth([FromRoute] string year)
        {
            var now = DateTime.UtcNow;
            var startOfYear = year switch
            {
                "current" => new DateTime(now.Year, 1, 1),
                "previous" => new DateTime(now.Year, 1, 1).AddYears(-1),
            };
            var result = await _mediator.Send(new GetExpensesByMonth { StartOfYear = startOfYear });
            return Ok(result);
        }

        [HttpGet("expenses-by-shop/{month}/month")]
        public async Task<ActionResult<PurchaseStatistic>> GetExpensesByShop([FromRoute] string month)
        {
            var now = DateTime.UtcNow;
            var startOfMonth = month switch
            {
                "current" => new DateTime(now.Year, now.Month, 1),
                "previous" => new DateTime(now.Year, now.Month, 1).AddMonths(-1),
            };
            var result = await _mediator.Send(new GetExpensesByShop { StartOfMonth = startOfMonth });
            return Ok(result);
        }

        [HttpGet("product-cost-change")]
        public async Task<ActionResult<PurchaseStatistic>> GetProductCostChange([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string orderBy)
        {
            var result = await _mediator.Send(new GetProductCostChange { Page = page, PageSize = pageSize, OrderBy = orderBy });
            return Ok(result);
        }
    }
}