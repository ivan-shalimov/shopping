using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shopping.Services.Common;
using Shopping.Shared.Models.Common;

namespace Shopping.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static ScopedRegistration<TRequest, TSuccessResponse> RegisterScopedRequest<TRequest, TSuccessResponse>(this IServiceCollection services)
            where TRequest : IRequest<Either<Fail, TSuccessResponse>>
        {
            return new ScopedRegistration<TRequest, TSuccessResponse>(services);
        }

        public static ScopedRegistration<TRequest, Success> RegisterScopedRequest<TRequest>(this IServiceCollection services)
           where TRequest : IRequest<Either<Fail, Success>>
        {
            return new ScopedRegistration<TRequest, Success>(services);
        }

        public static void RegisterScopedHandler<TRequest, TResult, THandler>(this IServiceCollection services)
           where TRequest : IRequest<TResult>
           where THandler : class, IRequestHandler<TRequest, TResult>
        {
            services.AddScoped<IPipelineBehavior<TRequest, TResult>, SimpleMediatorPipelineBehavior<TRequest, TResult>>();

            services.AddScoped<IRequestHandler<TRequest, TResult>, THandler>();
        }

        public static void RegisterScopedHandler<TRequest, THandler>(this IServiceCollection services)
           where TRequest : IRequest<Success>
           where THandler : class, IRequestHandler<TRequest, Success>
        {
            services.AddScoped<IPipelineBehavior<TRequest, Success>, SimpleMediatorPipelineBehavior<TRequest, Success>>();

            services.AddScoped<IRequestHandler<TRequest, Success>, THandler>();
        }
    }
}