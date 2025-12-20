using Microsoft.Extensions.DependencyInjection;
using Shopping.Shared.Models.Common;

namespace Shopping.Mediator
{
    public sealed class ScopedRegistration<TRequest, TSuccessResponse> : IRegisterHander<TRequest, TSuccessResponse>
        where TRequest : IRequest<Either<Fail, TSuccessResponse>>
    {
        private readonly IServiceCollection _services;

        public ScopedRegistration(IServiceCollection services)
        {
            _services = services;
        }

        public IRegisterHander<TRequest, TSuccessResponse> WithValidation<TValidator>()
            where TValidator : class, IRequestVaidator<TRequest>
        {
            _services.AddScoped<IRequestVaidator<TRequest>, TValidator>();
            return this;
        }

        public void ForHandler<THandler>()
            where THandler : class, IRequestHandler<TRequest, Either<Fail, TSuccessResponse>>
        {
            _services.AddScoped<IRequestHandler<TRequest, Either<Fail, TSuccessResponse>>, THandler>();
        }
    }

    public interface IRegisterHander<TRequest, TSuccessResponse>
        where TRequest : IRequest<Either<Fail, TSuccessResponse>>
    {
        void ForHandler<THandler>()
            where THandler : class, IRequestHandler<TRequest, Either<Fail, TSuccessResponse>>;
    }
}