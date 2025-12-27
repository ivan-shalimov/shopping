using Microsoft.Extensions.DependencyInjection;
using Shopping.Shared.Models.Common;

namespace Shopping.Mediator
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMediator(this IServiceCollection services)
        {
            services.AddScoped<IMediator, Mediator>();
        }

        public static ScopedRegistration<TRequest, TSuccessResponse> RegisterScopedRequestWithResult<TRequest, TSuccessResponse>(this IServiceCollection services)
            where TRequest : IRequest<Either<Fail, TSuccessResponse>>
        {
            return new ScopedRegistration<TRequest, TSuccessResponse>(services);
        }

        public static ScopedRegistration<TRequest, Success> RegisterScopedRequest<TRequest>(this IServiceCollection services)
           where TRequest : IRequest<Either<Fail, Success>>
        {
            return new ScopedRegistration<TRequest, Success>(services);
        }
    }
}