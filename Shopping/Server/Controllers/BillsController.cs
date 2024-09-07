using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopping.Server.Common;
using Shopping.Shared.Requests.Bills;

namespace Shopping.Server.Controllers
{
    [Route("api/bills")]
    [ApiController]
    public class BillsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BillsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetBills([FromQuery] GetBills request)
        {
            var result = await _mediator.Send(request).ConfigureAwait(false);
            return result.Reduce();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBill(CreateBill request)
        {
            var result = await _mediator.Send(request).ConfigureAwait(false);
            return result.Reduce();
        }

        [HttpDelete("{billId}")]
        public async Task<IActionResult> DeleteBill(Guid billId)
        {
            var result = await _mediator.Send(new DeleteBill { Id = billId }).ConfigureAwait(false);
            return result.Reduce();
        }

        [HttpGet("{billId}/items")]
        public async Task<IActionResult> GetBillItems(Guid billId)
        {
            var result = await _mediator.Send(new GetBillItems { BillId = billId }).ConfigureAwait(false);
            return result.Reduce();
        }

        [HttpPut("{billId}/items/{id}/quantity")]
        public async Task<IActionResult> UpdateBillItemQuantity(Guid billId, Guid id, UpdateBillItemQuantity request)
        {
            request.Id = id;
            request.BillId = billId;
            var result = await _mediator.Send(request);
            return result.Reduce();
        }
    }
}