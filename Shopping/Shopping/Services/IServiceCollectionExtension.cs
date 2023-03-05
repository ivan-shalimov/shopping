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
            //services.AddTransient<ServiceFactory>(p => p.GetService);
            services.AddTransient<IMediator, Mediator>();
        }

        public static void RegisterMediatrServices(this IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<GetPurchaseStatistic, PurchaseStatistic>, GetPurchaseStatisticHandler>();

            services.AddScoped<IRequestHandler<GetProducts, ProductModel[]>, GetProductsHandler>();
            services.AddScoped<IRequestHandler<AddProduct>, AddProductHandler>();
            services.AddScoped<IRequestHandler<UpdateProduct>, UpdateProductHandler>();
            services.AddScoped<IRequestHandler<DeleteProduct, Either<Fail, Success>>, DeleteProductHandler>();

            services.AddScoped<IRequestHandler<DeleteProduct, Either<Fail, Success>>, DeleteProductHandler>();
            services.AddScoped<IValidator<DeleteProduct>, DeleteProductValidator>();
            services.AddScoped<IPipelineBehavior<DeleteProduct, Either<Fail, Success>>, ValidationPipelineBehavior<DeleteProduct, Success, DeleteProductValidator>>();

            services.AddScoped<IRequestHandler<MergeProduct, Either<Fail, Success>>, MergeProductHandler>();
            services.AddScoped<IValidator<MergeProduct>, MergeProductValidator>();
            services.AddScoped<IPipelineBehavior<MergeProduct, Either<Fail, Success>>, ValidationPipelineBehavior<MergeProduct, Success, MergeProductValidator>>();

            RegisterProductKindsServices(services);

            services.AddScoped<IRequestHandler<GetReceipts, ReceiptModel[]>, GetReceiptsHandler>();
            services.AddScoped<IRequestHandler<AddReceipt, Either<Fail, Success>>, AddReceiptHandler>();
            services.AddScoped<IValidator<AddReceipt>, AddReceiptValidator>();
            services.AddScoped<IPipelineBehavior<AddReceipt, Either<Fail, Success>>, ValidationPipelineBehavior<AddReceipt, Success, AddReceiptValidator>>();
            services.AddScoped<IRequestHandler<UpdateReceipt, Either<Fail, Success>>, UpdateReceiptHandler>();
            services.AddScoped<IValidator<UpdateReceipt>, UpdateReceiptValidator>();
            services.AddScoped<IPipelineBehavior<UpdateReceipt, Either<Fail, Success>>, ValidationPipelineBehavior<UpdateReceipt, Success, UpdateReceiptValidator>>();
            services.AddScoped<IRequestHandler<UpdateReceiptTotal>, UpdateReceiptTotalHandler>();

            services.AddScoped<IRequestHandler<GetReceiptItems, ReceiptItemModel[]>, GetReceiptItemsHandler>();
            services.AddScoped<IRequestHandler<AddReceiptItem, Either<Fail, Success>>, AddReceiptItemHandler>();
            services.AddScoped<IValidator<AddReceiptItem>, AddReceiptItemValidator>();
            services.AddScoped<IPipelineBehavior<AddReceiptItem, Either<Fail, Success>>, ValidationPipelineBehavior<AddReceiptItem, Success, AddReceiptItemValidator>>();
            services.AddScoped<IRequestHandler<UpdateReceiptItem, Either<Fail, Success>>, UpdateReceiptItemHandler>();
            services.AddScoped<IValidator<UpdateReceiptItem>, UpdateReceiptItemValidator>();
            services.AddScoped<IPipelineBehavior<UpdateReceiptItem, Either<Fail, Success>>, ValidationPipelineBehavior<UpdateReceiptItem, Success, UpdateReceiptItemValidator>>();
            services.AddScoped<IRequestHandler<DeleteReceiptItem>, DeleteReceiptItemHandler>();
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