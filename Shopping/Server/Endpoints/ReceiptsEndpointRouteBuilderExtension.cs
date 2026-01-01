using Microsoft.AspNetCore.Mvc;
using Shopping.Mediator;
using Shopping.Server.Common;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Models.Results;
using Shopping.Shared.Requests;

namespace Shopping.Server.Endpoints
{
    public static class ReceiptsEndpointRouteBuilderExtension
    {
        public static void MapReceiptsEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/receipts", async ([FromQuery] int? year, [FromQuery] int month, [FromServices] IMediator mediator) =>
            {
                var result = await mediator
                .ExecuteAndReceive<GetReceipts, ReceiptModel[]>(new GetReceipts { Year = year ?? DateTime.UtcNow.Year, Month = month })
                .ConfigureAwait(false);
                return result.Reduce();
            });

            app.MapPost("api/receipts", async ([FromBody] AddReceipt request, [FromServices] IMediator mediator) =>
            {
                var result = await mediator
                .Execute(request)
                .ConfigureAwait(false);
                return result.Reduce();
            });

            app.MapPut("api/receipts/{id}", async (Guid id, [FromBody] UpdateReceipt request, [FromServices] IMediator mediator) =>
            {
                request.Id = id;
                var result = await mediator.Execute(request)
                .ConfigureAwait(false);
                return result.Reduce();
            });
        }

        public static void MapReceiptItemssEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/receipts/{receiptId}/items", async (Guid receiptId, [FromServices] IMediator mediator) =>
            {
                var result = await mediator
                .ExecuteAndReceive<GetReceiptItems, ReceiptItemModel[]>(new GetReceiptItems { ReceiptId = receiptId })
                .ConfigureAwait(false);
                return result.Reduce();
            });

            app.MapPost("api/receipts/{receiptId}/items", async (Guid receiptId, [FromBody] AddReceiptItem request, [FromServices] IMediator mediator) =>
            {
                request.ReceiptId = receiptId;
                var result = await mediator.Execute(request).ConfigureAwait(false);
                await mediator.ExecuteAndReceive<UpdateReceiptTotal, Success>(new UpdateReceiptTotal { Id = receiptId }).ConfigureAwait(false);
                return result.Reduce();
            });

            app.MapPut("api/receipts/{receiptId}/items/{id}", async (Guid receiptId, Guid id, [FromBody] UpdateReceiptItem request, [FromServices] IMediator mediator) =>
            {
                request.Id = id;
                request.ReceiptId = receiptId;
                var result = await mediator.Execute(request).ConfigureAwait(false);
                await mediator.ExecuteAndReceive<UpdateReceiptTotal, Success>(new UpdateReceiptTotal { Id = receiptId });
                return result.Reduce();
            });

            app.MapDelete("api/receipts/{receiptId}/items/{id}", async (Guid receiptId, Guid id, [FromServices] IMediator mediator) =>
            {
                await mediator.ExecuteAndReceive<DeleteReceiptItem, Success>(new DeleteReceiptItem { Id = id });
                await mediator.ExecuteAndReceive<UpdateReceiptTotal, Success>(new UpdateReceiptTotal { Id = receiptId }).ConfigureAwait(false);
                return Results.Ok();
            });
        }
    }
}