using Microsoft.AspNetCore.Mvc;
using Shopping.Mediator;
using Shopping.Server.Common;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Models.Results;
using Shopping.Shared.Requests;

namespace Shopping.Server.Endpoints
{
    public static class ProductsEndpointRouteBuilderExtension
    {
        public static void MapProductsEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/products", async ([FromQuery] Guid? productKindId, [FromQuery] bool? showHidden, [FromServices] IMediator mediator) =>
            {
                var result = await mediator
                .ExecuteAndReceive<GetProducts, ProductModel[]>(new GetProducts { ProductKindId = productKindId, ShowHidden = showHidden ?? false })
                .ConfigureAwait(false);
                return result.Reduce();
            });

            app.MapPost("api/products", async ([FromBody] AddProduct request, [FromServices] IMediator mediator) =>
            {
                var result = await mediator
                .ExecuteAndReceive<AddProduct, Success>(request)
                .ConfigureAwait(false);
                return result.Reduce();
            });

            app.MapPut("api/products/{id}", async (Guid id, [FromBody] UpdateProduct request, [FromServices] IMediator mediator) =>
            {
                request.Id = id;
                var result = await mediator.ExecuteAndReceive<UpdateProduct, Success>(request)
                .ConfigureAwait(false);
                return result.Reduce();
            });

            app.MapDelete("api/products/{id}", async (Guid id, [FromServices] IMediator mediator) =>
            {
                var result = await mediator
                .Execute(new DeleteProduct { Id = id })
                .ConfigureAwait(false);
                return result.Reduce();
            });

            app.MapPut("api/products/{id}/hidden", async (Guid id, [FromServices] IMediator mediator) =>
            {
                var result = await mediator
                .Execute(new ChangeProductVisibility { Id = id, Hidden = true })
                .ConfigureAwait(false);
                return result.Reduce();
            });

            app.MapPut("api/products/{id}/visible", async (Guid id, [FromServices] IMediator mediator) =>
            {
                var result = await mediator
                .Execute(new ChangeProductVisibility { Id = id, Hidden = false })
                .ConfigureAwait(false);
                return result.Reduce();
            });

            app.MapPost("api/products/merged", async ([FromBody] MergeProduct request, [FromServices] IMediator mediator) =>
            {
                var result = await mediator
                .Execute(request)
                .ConfigureAwait(false);
                return result.Reduce();
            });
        }

        public static void MapProductKindssEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/products/kinds", async ([FromServices] IMediator mediator) =>
            {
                var result = await mediator
                .ExecuteAndReceive<GetProductKinds, ProductKindModel[]>(new GetProductKinds())
                .ConfigureAwait(false);
                return result.Reduce();
            });

            app.MapPost("api/products/kinds", async ([FromBody] AddProductKind request, [FromServices] IMediator mediator) =>
            {
                var result = await mediator
                .Execute(request)
                .ConfigureAwait(false);
                return result.Reduce();
            });

            app.MapPost("api/products/kinds/merged", async ([FromBody] MergeProductKind request, [FromServices] IMediator mediator) =>
            {
                var result = await mediator
                .Execute(request)
                .ConfigureAwait(false);
                return result.Reduce();
            });

            app.MapPut("api/products/kinds/{id}", async (Guid id, [FromBody] UpdateProductKind request, [FromServices] IMediator mediator) =>
            {
                request.Id = id;
                var result = await mediator.Execute(request)
                .ConfigureAwait(false);
                return result.Reduce();
            });

            app.MapDelete("api/products/kinds/{id}", async (Guid id, [FromServices] IMediator mediator) =>
            {
                var result = await mediator
                .Execute(new DeleteProductKind { Id = id })
                .ConfigureAwait(false);
                return result.Reduce();
            });
        }
    }
}