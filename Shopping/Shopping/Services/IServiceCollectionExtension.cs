using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shopping.Services.Extensions;
using Shopping.Services.Handlers;
using Shopping.Services.Handlers.Prices;
using Shopping.Services.Handlers.Statistic;
using Shopping.Services.Validators;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Models.Results;
using Shopping.Shared.Requests;
using Shopping.Shared.Requests.Prices;
using Shopping.Shared.Requests.Statistic;

namespace Shopping.Services
{
    public static class IServiceCollectionExtension
    {
        public static void RegisterMediatR(this IServiceCollection services)
        {
            //services.AddTransient<ServiceFactory>(p => p.GetService);
            services.AddTransient<IMediator, Mediator>();
        }

        public static void RegisterMediatrServices(this IServiceCollection services)
        {
            services.RegisterScopedHandler<GetPurchaseStatistic, PurchaseStatistic, GetPurchaseStatisticHandler>();

            services.RegisterScopedHandler<GetPurchaseStatistic, PurchaseStatistic, GetPurchaseStatisticHandler>();

            services.RegisterScopedHandler<GetLastProductsPrices, IDictionary<Guid, decimal>, GetLastProductsPricesHandler>();

            services.RegisterScopedHandler<GetExpensesByKind, IDictionary<string, decimal>, GetExpensesByKindHandler>();
            services.RegisterScopedHandler<GetExpensesByProducts, IDictionary<string, decimal>, GetExpensesByProductsHandler>();
            services.RegisterScopedHandler<GetProductsExpensesDetails, ProductExpensesDetail[], GetProductsExpensesDetailsHandler>();
            services.RegisterScopedHandler<GetExpensesByMonth, IDictionary<int, decimal>, GetExpensesByMonthHandler>();
            services.RegisterScopedHandler<GetExpensesByShop, IDictionary<string, decimal>, GetExpensesByShopHandler>();
            services.RegisterScopedHandler<GetProductCostChange, ProductCostChange[], GetProductCostChangeHandler>();

            services.RegisterScopedHandler<GetProducts, ProductModel[], GetProductsHandler>();
            services.RegisterScopedHandler<AddProduct, AddProductHandler>();
            services.RegisterScopedHandler<UpdateProduct, UpdateProductHandler>();
            services.RegisterScopedHandler<ChangeProductVisibility, Either<Fail, Success>, ChangeProductVisibilityHandler>();
            services.RegisterScopedHandler<DeleteProduct, Either<Fail, Success>, DeleteProductHandler>();
            services.RegisterScopedHandler<GetReceipts, ReceiptModel[], GetReceiptsHandler>();

            RegisterProductKindsServices(services);

            services.RegisterScopedRequest<DeleteProduct>()
                .WithValidation<DeleteProductValidator>()
                .ForHandler<DeleteProductHandler>();

            services.RegisterScopedRequest<MergeProduct>()
                .WithValidation<MergeProductValidator>()
                .ForHandler<MergeProductHandler>();

            services.RegisterScopedRequest<AddReceipt>()
                .WithValidation<AddReceiptValidator>()
                .ForHandler<AddReceiptHandler>();

            services.RegisterScopedRequest<UpdateReceipt>()
                .WithValidation<UpdateReceiptValidator>()
                .ForHandler<UpdateReceiptHandler>();

            services.RegisterScopedHandler<UpdateReceiptTotal, UpdateReceiptTotalHandler>();

            services.RegisterScopedHandler<GetReceiptItems, ReceiptItemModel[], GetReceiptItemsHandler>();
            services.RegisterScopedHandler<AddReceiptItem, Either<Fail, Success>, AddReceiptItemHandler>();

            services.RegisterScopedRequest<AddReceiptItem>()
                .WithValidation<AddReceiptItemValidator>()
                .ForHandler<AddReceiptItemHandler>();

            services.RegisterScopedRequest<UpdateReceiptItem>()
                .WithValidation<UpdateReceiptItemValidator>()
                .ForHandler<UpdateReceiptItemHandler>();

            services.RegisterScopedHandler<DeleteReceiptItem, DeleteReceiptItemHandler>();
        }

        private static void RegisterProductKindsServices(IServiceCollection services)
        {
            services.RegisterScopedHandler<GetProductKinds, ProductKindModel[], GetProductKindsHandler>();

            services.RegisterScopedRequest<AddProductKind>()
                .WithValidation<AddProductKindValidator>()
                .ForHandler<AddProductKindHandler>();

            services.RegisterScopedRequest<UpdateProductKind>()
                .WithValidation<UpdateProductKindValidator>()
                .ForHandler<UpdateProductKindHandler>();

            services.RegisterScopedRequest<DeleteProductKind>()
                .WithValidation<DeleteProductKindValidator>()
                .ForHandler<DeleteProductKindHandler>();

            services.RegisterScopedRequest<MergeProductKind>()
                .WithValidation<MergeProductKindValidator>()
                .ForHandler<MergeProductKindHandler>();
        }
    }
}