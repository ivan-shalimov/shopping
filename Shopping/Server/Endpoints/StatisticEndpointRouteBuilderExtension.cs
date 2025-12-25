using Microsoft.AspNetCore.Mvc;
using Shopping.Mediator;
using Shopping.Shared.Models.Results;
using Shopping.Shared.Requests.Statistic;

namespace Shopping.Server.Endpoints
{
    public static class StatisticEndpointRouteBuilderExtension
    {
        public static void MapstatisticEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/statistic/expenses-by-kinds", async ([FromQuery] DateTime start, [FromQuery] DateTime end, [FromServices] IMediator mediator) =>
            {
                var result = await mediator
                .ExecuteAndReceiveWithoutValidation<GetExpensesByKind, IDictionary<string, decimal>>(new GetExpensesByKind { Start = start, End = end })
                .ConfigureAwait(false);
                return Results.Ok(result);
            });

            app.MapGet("api/statistic/expenses-by-products", async ([FromQuery] string kind, [FromQuery] DateTime start, [FromQuery] DateTime end, [FromServices] IMediator mediator) =>
            {
                var result = await mediator
                .ExecuteAndReceiveWithoutValidation<GetExpensesByProducts, IDictionary<string, decimal>>(new GetExpensesByProducts { Kind = kind, Start = start, End = end })
                .ConfigureAwait(false);
                return Results.Ok(result);
            });

            app.MapGet("api/statistic/product-expenses-details", async ([FromQuery] string productName, [FromQuery] DateTime start, [FromQuery] DateTime end, [FromServices] IMediator mediator) =>
            {
                var result = await mediator
                .ExecuteAndReceiveWithoutValidation<GetProductsExpensesDetails, ProductExpensesDetail[]>(new GetProductsExpensesDetails { ProductName = productName, Start = start, End = end })
                .ConfigureAwait(false);
                return Results.Ok(result);
            });

            app.MapGet("api/statistic/expenses-by-kind/{month}/month", async (string month, [FromServices] IMediator mediator) =>
            {
                var now = DateTime.UtcNow;
                var startOfMonth = month switch
                {
                    "current" => new DateTime(now.Year, now.Month, 1),
                    "previous" => new DateTime(now.Year, now.Month, 1).AddMonths(-1),
                };
                var result = await mediator
                .ExecuteAndReceiveWithoutValidation<GetExpensesByKind, IDictionary<string, decimal>>(
                    new GetExpensesByKind { OnlyMain = true, Start = startOfMonth, End = startOfMonth.AddMonths(1).AddSeconds(-1) })
                .ConfigureAwait(false);
                return Results.Ok(result);
            });

            app.MapGet("api/statistic/expenses-by-month/{year}/year", async (string year, [FromServices] IMediator mediator) =>
            {
                var now = DateTime.UtcNow;
                var startOfYear = year switch
                {
                    "current" => new DateTime(now.Year, 1, 1),
                    "previous" => new DateTime(now.Year, 1, 1).AddYears(-1),
                };
                var result = await mediator
                .ExecuteAndReceiveWithoutValidation<GetExpensesByMonth, IDictionary<int, decimal>>(new GetExpensesByMonth { StartOfYear = startOfYear })
                .ConfigureAwait(false);
                return Results.Ok(result);
            });

            app.MapGet("api/statistic/expenses-by-shop/{month}/month", async (string month, [FromServices] IMediator mediator) =>
            {
                var now = DateTime.UtcNow;
                var startOfMonth = month switch
                {
                    "current" => new DateTime(now.Year, now.Month, 1),
                    "previous" => new DateTime(now.Year, now.Month, 1).AddMonths(-1),
                };
                var result = await mediator
                .ExecuteAndReceiveWithoutValidation<GetExpensesByShop, IDictionary<string, decimal>>(new GetExpensesByShop { StartOfMonth = startOfMonth })
                .ConfigureAwait(false);
                return Results.Ok(result);
            });

            app.MapGet("api/statistic/product-cost-change", async ([FromQuery] int page, [FromQuery] int pageSize, [FromServices] IMediator mediator) =>
            {
                var result = await mediator
                .ExecuteAndReceiveWithoutValidation<GetProductCostChange, ProductCostChange[]>(new GetProductCostChange { Page = page, PageSize = pageSize })
                .ConfigureAwait(false);
                return Results.Ok(result);
            });
        }
    }
}