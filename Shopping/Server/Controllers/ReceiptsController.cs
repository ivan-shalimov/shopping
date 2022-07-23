using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopping.Shared.Models.Results;
using Shopping.Shared.Requests;

namespace Shopping.Server.Controllers
{
    [Route("api/receipts")]
    [ApiController]
    public class ReceiptsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReceiptsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<ReceiptModel[]>> GetReceipts([FromQuery] int month)
        {
            var result = await _mediator.Send(new GetReceipts { Month = month }).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddReceipt(AddReceipt request)
        {
            await _mediator.Send(request).ConfigureAwait(false);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, UpdateReceipt request)
        {
            request.Id = id;
            await _mediator.Send(request).ConfigureAwait(false);
            return Ok();
        }

        [HttpGet("{receiptId}/items")]
        public async Task<ActionResult<ReceiptItemModel[]>> GetReceiptItems(Guid receiptId)
        {
            var result = await _mediator.Send(new GetReceiptItems { ReceiptId = receiptId }).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpPost("{receiptId}/items")]
        public async Task<IActionResult> AddReceiptItem(Guid receiptId, AddReceiptItem request)
        {
            request.ReceiptId = receiptId;
            await _mediator.Send(request).ConfigureAwait(false);
            await _mediator.Send(new UpdateReceiptTotal { Id = receiptId }).ConfigureAwait(false);
            return Ok();
        }

        [HttpPut("{receiptId}/items/{id}")]
        public async Task<IActionResult> UpdateReceiptItem(Guid receiptId, Guid id, UpdateReceiptItem request)
        {
            request.Id = id;
            request.ReceiptId = receiptId;
            await _mediator.Send(request);
            await _mediator.Send(new UpdateReceiptTotal { Id = receiptId }).ConfigureAwait(false);
            return Ok();
        }

        [HttpDelete("{receiptId}/items/{id}")]
        public async Task<IActionResult> DeleteReceiptItem(Guid receiptId, Guid id)
        {
            await _mediator.Send(new DeleteReceiptItem { Id = id });
            await _mediator.Send(new UpdateReceiptTotal { Id = receiptId }).ConfigureAwait(false);
            return Ok();
        }
    }
}