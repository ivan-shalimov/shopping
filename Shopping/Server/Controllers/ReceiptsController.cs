using Microsoft.AspNetCore.Mvc;
using Shopping.Mediator;
using Shopping.Server.Common;
using Shopping.Shared.Models.Common;
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
            var result = await _mediator.ExecuteAndReceiveWithoutValidation<GetReceipts, ReceiptModel[]>(new GetReceipts { Month = month }).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddReceipt(AddReceipt request)
        {
            var result = await _mediator.Execute(request).ConfigureAwait(false);
            return result.Reduce();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, UpdateReceipt request)
        {
            request.Id = id;
            var result = await _mediator.Execute(request).ConfigureAwait(false);
            return result.Reduce();
        }

        [HttpGet("{receiptId}/items")]
        public async Task<ActionResult<ReceiptItemModel[]>> GetReceiptItems(Guid receiptId)
        {
            var result = await _mediator.ExecuteAndReceiveWithoutValidation<GetReceiptItems, ReceiptItemModel[]>(new GetReceiptItems { ReceiptId = receiptId }).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpPost("{receiptId}/items")]
        public async Task<IActionResult> AddReceiptItem(Guid receiptId, AddReceiptItem request)
        {
            request.ReceiptId = receiptId;
            var result = await _mediator.Execute(request).ConfigureAwait(false);
            await _mediator.ExecuteAndReceiveWithoutValidation<UpdateReceiptTotal, Success>(new UpdateReceiptTotal { Id = receiptId }).ConfigureAwait(false);
            return result.Reduce();
        }

        [HttpPut("{receiptId}/items/{id}")]
        public async Task<IActionResult> UpdateReceiptItem(Guid receiptId, Guid id, UpdateReceiptItem request)
        {
            request.Id = id;
            request.ReceiptId = receiptId;
            var result = await _mediator.Execute(request);
            await _mediator.ExecuteAndReceiveWithoutValidation<UpdateReceiptTotal, Success>(new UpdateReceiptTotal { Id = receiptId }).ConfigureAwait(false);
            return result.Reduce();
        }

        [HttpDelete("{receiptId}/items/{id}")]
        public async Task<IActionResult> DeleteReceiptItem(Guid receiptId, Guid id)
        {
            await _mediator.ExecuteAndReceiveWithoutValidation<DeleteReceiptItem, Success>(new DeleteReceiptItem { Id = id });
            await _mediator.ExecuteAndReceiveWithoutValidation<UpdateReceiptTotal, Success>(new UpdateReceiptTotal { Id = receiptId }).ConfigureAwait(false);
            return Ok();
        }
    }
}