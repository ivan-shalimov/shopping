using Microsoft.AspNetCore.Mvc;
using Shopping.Mediator;
using Shopping.Server.Common;
using Shopping.Shared.Models.Results;
using Shopping.Shared.Requests;

namespace Shopping.Server.Endpoints
{
    public static class CarCostsEndpointRouteBuilderExtension
    {
        public static void MapCarCostsEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/car-costs", async ([FromQuery] int month, [FromServices] IMediator mediator) =>
            {
                var result = await mediator.ExecuteAndReceive<GetCarCosts, CarCostModel[]>(new GetCarCosts { Month = month }).ConfigureAwait(false);
                return result.Reduce();
            });

            app.MapPost("api/car-costs", async ([FromBody] AddCarCost request, [FromServices] IMediator mediator) =>
            {
                var result = await mediator
                .Execute(request)
                .ConfigureAwait(false);
                return result.Reduce();
            });

            app.MapPut("api/car-costs/{id}", async (Guid id, [FromBody] UpdateCarCost request, [FromServices] IMediator mediator) =>
            {
                request.Id = id;
                var result = await mediator.Execute(request)
                .ConfigureAwait(false);
                return result.Reduce();
            });

            app.MapDelete("api/car-costs/{id}", async (Guid id, [FromServices] IMediator mediator) =>
            {
                var result = await mediator
                .Execute(new DeleteCarCost { Id = id })
                .ConfigureAwait(false);
                return result.Reduce();
            });
        }
    }
}