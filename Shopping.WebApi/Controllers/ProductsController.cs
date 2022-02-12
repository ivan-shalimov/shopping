using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopping.Models.Results;
using Shopping.Requests;

namespace Shopping.WebApi.Controllers
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
        public async Task<ActionResult<ProductModel[]>> GetProducts()
        {
            var result = await _mediator.Send(new GetProducts());
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

        [HttpGet("kinds")]
        public async Task<ActionResult<ProductKindModel[]>> GetProductKinds()
        {
            var result = await _mediator.Send(new GetProductKinds());
            return Ok(result);
        }

        [HttpPost("kinds")]
        public async Task<IActionResult> AddProductKind(AddProductKind request)
        {
            await _mediator.Send(request);
            return Ok();
        }

        [HttpPut("kinds/{id}")]
        public async Task<IActionResult> UpdateProductKind(Guid id, UpdateProductKind request)
        {
            request.Id = id;
            await _mediator.Send(request);
            return Ok();
        }
    }
}