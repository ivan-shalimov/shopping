using Microsoft.AspNetCore.Mvc;
using Shopping.Mediator;
using Shopping.Server.Common;
using Shopping.Shared.Models.Results;
using Shopping.Shared.Requests;
using Shopping.Shared.Requests.Prices;

namespace Shopping.Server.Endpoints
{
    public static class EndpointRouteBuilderExtension
    {
        public static void MapEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapBillsEndpoints();
            app.MapPricesEndpoints();
            app.MapPurchasesEndpoints();
            app.MapCarCostsEndpoints();
            app.MapProductsEndpoints();
            app.MapProductKindssEndpoints();
            app.MapReceiptsEndpoints();
            app.MapReceiptItemssEndpoints();
            app.MapstatisticEndpoints();
        }

        private static void MapPurchasesEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/purchases/statistic", async ([FromQuery] int month, [FromServices] IMediator mediator) =>
            {
                var result = await mediator
                .ExecuteAndReceive<GetPurchaseStatistic, PurchaseStatistic>(
                    new GetPurchaseStatistic { Month = month })
                .ConfigureAwait(false);
                return result.Reduce();
            });
        }

        private static void MapPricesEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/prices/latest", async ([FromQuery] Guid receiptId, [FromQuery] Guid[] productIds, [FromServices] IMediator mediator) =>
            {
                var result = await mediator
                .ExecuteAndReceive<GetLastProductsPrices, IDictionary<Guid, decimal>>(
                    new GetLastProductsPrices { ReceiptId = receiptId, ProductIds = productIds })
                .ConfigureAwait(false);
                return result.Reduce();
            });
        }
    }
}