using Microsoft.Extensions.DependencyInjection;
using Shopping.Services.Common;
using Shopping.Services.Interfaces;

namespace Shopping.Mediator
{
    public static class IServiceCollectionExtension
    {
        public static void AddMediator(this IServiceCollection services)
        {
            services.AddSingleton<IMediator, Mediator>();
        }
    }
}