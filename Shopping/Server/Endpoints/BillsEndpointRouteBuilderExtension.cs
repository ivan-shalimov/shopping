using Microsoft.AspNetCore.Mvc;
using Shopping.Mediator;
using Shopping.Server.Common;
using Shopping.Shared.Models.Results;
using Shopping.Shared.Requests.Bills;

namespace Shopping.Server.Endpoints
{
    public static class BillsEndpointRouteBuilderExtension
    {
        public static void MapBillsEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/bills", async ([FromServices] IMediator mediator) =>
            {
                var result = await mediator.ExecuteAndReceive<GetBills, BillModel[]>(new GetBills {}).ConfigureAwait(false);
                return result.Reduce();
            });

            app.MapPost("api/bills", async ([FromBody] CreateBill request, [FromServices] IMediator mediator) =>
            {
                var result = await mediator
                .Execute(request)
                .ConfigureAwait(false);
                return result.Reduce();
            });

            app.MapDelete("api/bills/{billId}", async (Guid billId, [FromServices] IMediator mediator) =>
            {
                var result = await mediator
                .Execute(new DeleteBill { Id = billId })
                .ConfigureAwait(false);
                return result.Reduce();
            });

            app.MapGet("api/bills/{billId}/items", async (Guid billId, [FromServices] IMediator mediator) =>
            {
                var result = await mediator
                .ExecuteAndReceive<GetBillItems, BillItemModel[]>(new GetBillItems { BillId = billId })
                .ConfigureAwait(false);
                return result.Reduce();
            });

            app.MapPut("api/bills/{billId}/items/{id}/quantity", async (Guid billId, Guid id, [FromBody] UpdateBillItemQuantity request, [FromServices] IMediator mediator) =>
            {
                request.Id = id;
                request.BillId = billId;
                var result = await mediator.Execute(request)
                .ConfigureAwait(false);
                return result.Reduce();
            });

            app.MapPut("api/bills/{billId}/items/{id}/rate", async (Guid billId, Guid id, [FromBody] UpdateBillItemRate request, [FromServices] IMediator mediator) =>
            {
                request.Id = id;
                request.BillId = billId;
                var result = await mediator.Execute(request)
                .ConfigureAwait(false);
                return result.Reduce();
            });
        }
    }
}