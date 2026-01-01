using Microsoft.Extensions.DependencyInjection;
using Shopping.Mediator;
using Shopping.Models.Requests;
using Shopping.Services.Common;
using Shopping.Services.Handlers;
using Shopping.Services.Handlers.CarCosts;
using Shopping.Services.Handlers.Maintenance;
using Shopping.Services.Handlers.Prices;
using Shopping.Services.Handlers.Statistic;
using Shopping.Services.Interfaces;
using Shopping.Services.Validators;
using Shopping.Services.Validators.Bills;
using Shopping.Shared.Models.Results;
using Shopping.Shared.Requests;
using Shopping.Shared.Requests.Bills;
using Shopping.Shared.Requests.Prices;
using Shopping.Shared.Requests.Statistic;

namespace Shopping.Services
{
    public static class IServiceCollectionExtension
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IBackgroundMediatorRequestHandler, BackgroundMediatorRequestHandler>();
        }

        public static void RegisterMediatR(this IServiceCollection services)
        {
            services.AddMediator();
        }

        public static void RegisterMediatrServices(this IServiceCollection services)
        {
            services.RegisterScopedRequest<RecalculateTotal>()
                .ForHandler<RecalculateTotalHandler>();

            services.RegisterScopedRequestWithResult<GetReceipts, ReceiptModel[]>()
                .ForHandler<GetReceiptsHandler>();

            services.RegisterScopedRequestWithResult<GetReceiptItems, ReceiptItemModel[]>()
                .ForHandler<GetReceiptItemsHandler>();

            services.RegisterScopedRequestWithResult<GetPurchaseStatistic, PurchaseStatistic>()
                .ForHandler<GetPurchaseStatisticHandler>();

            services.RegisterScopedRequestWithResult<GetProducts, ProductModel[]>()
                .ForHandler<GetProductsHandler>();

            services.RegisterScopedRequestWithResult<GetProductKinds, ProductKindModel[]>()
                .ForHandler<GetProductKindsHandler>();

            services.RegisterScopedRequestWithResult<GetCarCosts, CarCostModel[]>()
                .ForHandler<GetCarCostsHandler>();

            services.RegisterScopedRequestWithResult<GetLastProductsPrices, IDictionary<Guid, decimal>>()
                .ForHandler<GetLastProductsPricesHandler>();

            services.RegisterScopedRequestWithResult<GetExpensesByKind, IDictionary<string, decimal>>()
                .ForHandler<GetExpensesByKindHandler>();

            services.RegisterScopedRequestWithResult<GetExpensesByProducts, IDictionary<string, decimal>>()
                .ForHandler<GetExpensesByProductsHandler>();

            services.RegisterScopedRequestWithResult<GetProductsExpensesDetails, ProductExpensesDetail[]>()
                .ForHandler<GetProductsExpensesDetailsHandler>();

            services.RegisterScopedRequestWithResult<GetExpensesByMonth, IDictionary<int, decimal>>()
                .ForHandler<GetExpensesByMonthHandler>();

            services.RegisterScopedRequestWithResult<GetExpensesByShop, IDictionary<string, decimal>>()
                .ForHandler<GetExpensesByShopHandler>();

            services.RegisterScopedRequestWithResult<GetProductCostChange, ProductCostChange[]>()
                .ForHandler<GetProductCostChangeHandler>();

            services.RegisterScopedRequest<AddProduct>()
                .ForHandler<AddProductHandler>();

            services.RegisterScopedRequest<UpdateProduct>()
                .ForHandler<UpdateProductHandler>();

            RegisterProductKindsServices(services);
            RegisterCarCostsServices(services);
            RegisterBillsServices(services);

            services.RegisterScopedRequest<ChangeProductVisibility>()
                .ForHandler<ChangeProductVisibilityHandler>();

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

            services.RegisterScopedRequest<UpdateReceiptTotal>()
                .ForHandler<UpdateReceiptTotalHandler>();

            services.RegisterScopedRequest<AddReceiptItem>()
                .WithValidation<AddReceiptItemValidator>()
                .ForHandler<AddReceiptItemHandler>();

            services.RegisterScopedRequest<UpdateReceiptItem>()
                .WithValidation<UpdateReceiptItemValidator>()
                .ForHandler<UpdateReceiptItemHandler>();

            services.RegisterScopedRequest<DeleteReceiptItem>()
                .ForHandler<DeleteReceiptItemHandler>();

            services.RegisterScopedRequest<UpdatePriceChangeProjection>()
                .ForHandler<UpdatePriceChangeProjectionHandler>();
        }

        private static void RegisterProductKindsServices(IServiceCollection services)
        {
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

        private static void RegisterCarCostsServices(IServiceCollection services)
        {
            services.RegisterScopedRequest<AddCarCost>()
                  .WithValidation<AddCarCostValidator>()
                  .ForHandler<AddCarCostHandler>();

            services.RegisterScopedRequest<UpdateCarCost>()
               .WithValidation<UpdateCarCostValidator>()
               .ForHandler<UpdateCarCostHandler>();

            services.RegisterScopedRequest<DeleteCarCost>()
                .WithValidation<DeleteCarCostValidator>()
                .ForHandler<DeleteCarCostHandler>();
        }

        private static void RegisterBillsServices(IServiceCollection services)
        {
            services.RegisterScopedRequestWithResult<GetBills, BillModel[]>()
                .ForHandler<GetBillsHandler>();

            services.RegisterScopedRequestWithResult<GetBillItems, BillItemModel[]>()
                .ForHandler<GetBillItemsHandler>();

            services.RegisterScopedRequest<CreateBill>()
                  .WithValidation<CreateBillValidator>()
                  .ForHandler<CreateBillHandler>();

            services.RegisterScopedRequest<UpdateBillItemQuantity>()
               .WithValidation<UpdateBillItemQuantityValidator>()
               .ForHandler<UpdateBillItemQuantityHandler>();

            services.RegisterScopedRequest<DeleteBill>()
                .WithValidation<DeleteBillValidator>()
                .ForHandler<DeleteBillHandler>();

            services.RegisterScopedRequest<UpdateBillTotal>()
                .ForHandler<UpdateBillTotalHandler>();
        }
    }
}