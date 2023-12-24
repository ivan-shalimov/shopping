using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shopping.Shared.Models.Common;

namespace Shopping.Services.Common
{
    public sealed class ScopedRegistration<TRequest, TSuccessResponse> : IRegisterHander<TRequest, TSuccessResponse>
        where TRequest : IRequest<Either<Fail, TSuccessResponse>>
    {
        private readonly IServiceCollection _services;

        public ScopedRegistration(IServiceCollection services)
        {
            _services = services;
            services.AddScoped<IPipelineBehavior<TRequest, Either<Fail, TSuccessResponse>>, MediatorPipelineBehavior<TRequest, TSuccessResponse>>();
        }

        public IRegisterHander<TRequest, TSuccessResponse> WithValidation<TValidator>()
            where TValidator : class, IValidator<TRequest>
        {
            _services.AddScoped<IValidator<TRequest>, TValidator>();
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