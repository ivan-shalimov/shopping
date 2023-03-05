using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopping.Server.Common;
using Shopping.Shared.Models.Results;
using Shopping.Shared.Requests;

namespace Shopping.Server.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<ProductModel[]>> GetProducts(Guid? productKindId)
        {
            var result = await _mediator.Send(new GetProducts { ProductKindId = productKindId });
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProduct request)
        {
            await _mediator.Send(request);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, UpdateProduct request)
        {
            request.Id = id;
            await _mediator.Send(request);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var result = await _mediator.Send(new DeleteProduct { Id = id });
            return result.Reduce();
        }

        [HttpPost("merged")]
        public async Task<IActionResult> MergeProductKind(MergeProduct request)
        {
            var result = await _mediator.Send(request);
            return result.Reduce();
        }

        [HttpGet("kinds")]
        public async Task<ActionResult<ProductKindModel[]>> GetProductKinds()
        {
            var result = await _mediator.Send(new GetProductKinds());
            return Ok(result);
        }

        [HttpPost("kinds")]
        public async Task<IActionResult> AddProductKind(AddProductKind request)
        {
            var result = await _mediator.Send(request);
            return result.Reduce();
        }

        [HttpPost("kinds/merged")]
        public async Task<IActionResult> MergeProductKind(MergeProductKind request)
        {
            var result = await _mediator.Send(request);
            return result.Reduce();
        }

        [HttpPut("kinds/{id}")]
        public async Task<IActionResult> UpdateProductKind(Guid id, UpdateProductKind request)
        {
            request.Id = id;
            var result = await _mediator.Send(request);
            return result.Reduce();
        }

        [HttpDelete("kinds/{id}")]
        public async Task<IActionResult> DeleteProductKind(Guid id)
        {
            var result = await _mediator.Send(new DeleteProductKind { Id = id });
            return result.Reduce();
        }
    }
}