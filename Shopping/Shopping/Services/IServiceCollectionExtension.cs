using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shopping.Services.Common;
using Shopping.Services.Handlers;
using Shopping.Services.Validators;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Models.Results;
using Shopping.Shared.Requests;

namespace Shopping.Services
{
    public static class IServiceCollectionExtension
    {
        public static void RegisterMediatR(this IServiceCollection services)
        {
            services.AddTransient<ServiceFactory>(p => p.GetService);
            services.AddTransient<IMediator, Mediator>();
        }

        public static void RegisterMediatrServices(this IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<GetPurchaseStatistic, PurchaseStatistic>, GetPurchaseStatisticHandler>();

            services.AddScoped<IRequestHandler<GetProducts, ProductModel[]>, GetProductsHandler>();
            services.AddScoped<IRequestHandler<AddProduct, Unit>, AddProductHandler>();
            services.AddScoped<IRequestHandler<UpdateProduct, Unit>, UpdateProductHandler>();
            services.AddScoped<IRequestHandler<DeleteProduct, Either<Fail, Success>>, DeleteProductHandler>();

            services.AddScoped<IRequestHandler<DeleteProduct, Either<Fail, Success>>, DeleteProductHandler>();
            services.AddScoped<IValidator<DeleteProduct>, DeleteProductValidator>();
            services.AddScoped<IPipelineBehavior<DeleteProduct, Either<Fail, Success>>, ValidationPipelineBehavior<DeleteProduct, Success, DeleteProductValidator>>();

            RegisterProductKindsServices(services);

            services.AddScoped<IRequestHandler<GetReceipts, ReceiptModel[]>, GetReceiptsHandler>();
            services.AddScoped<IRequestHandler<AddReceipt, Unit>, AddReceiptHandler>();
            services.AddScoped<IRequestHandler<UpdateReceipt, Unit>, UpdateReceiptHandler>();
            services.AddScoped<IRequestHandler<UpdateReceiptTotal, Unit>, UpdateReceiptTotalHandler>();

            services.AddScoped<IRequestHandler<GetReceiptItems, ReceiptItemModel[]>, GetReceiptItemsHandler>();
            services.AddScoped<IRequestHandler<AddReceiptItem, Unit>, AddReceiptItemHandler>();
            services.AddScoped<IRequestHandler<UpdateReceiptItem, Unit>, UpdateReceiptItemHandler>();
            services.AddScoped<IRequestHandler<DeleteReceiptItem, Unit>, DeleteReceiptItemHandler>();
        }

        private static void RegisterProductKindsServices(IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<GetProductKinds, ProductKindModel[]>, GetProductKindsHandler>();

            services.AddScoped<IRequestHandler<AddProductKind, Either<Fail, Success>>, AddProductKindHandler>();
            services.AddScoped<IValidator<AddProductKind>, AddProductKindValidator>();
            services.AddScoped<IPipelineBehavior<AddProductKind, Either<Fail, Success>>, ValidationPipelineBehavior<AddProductKind, Success, AddProductKindValidator>>();

            services.AddScoped<IValidator<UpdateProductKind>, UpdateProductKindValidator>();
            services.AddScoped<IPipelineBehavior<UpdateProductKind, Either<Fail, Success>>, ValidationPipelineBehavior<UpdateProductKind, Success, UpdateProductKindValidator>>();
            services.AddScoped<IRequestHandler<UpdateProductKind, Either<Fail, Success>>, UpdateProductKindHandler>();

            services.AddScoped<IRequestHandler<DeleteProductKind, Either<Fail, Success>>, DeleteProductKindHandler>();
            services.AddScoped<IValidator<DeleteProductKind>, DeleteProductKindValidator>();
            services.AddScoped<IPipelineBehavior<DeleteProductKind, Either<Fail, Success>>, ValidationPipelineBehavior<DeleteProductKind, Success, DeleteProductKindValidator>>();

            services.AddScoped<IRequestHandler<MergeProductKind, Either<Fail, Success>>, MergeProductKindHandler>();
            services.AddScoped<IValidator<MergeProductKind>, MergeProductKindValidator>();
            services.AddScoped<IPipelineBehavior<MergeProductKind, Either<Fail, Success>>, ValidationPipelineBehavior<MergeProductKind, Success, MergeProductKindValidator>>();
        }
    }
}