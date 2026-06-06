using OliveOilEshop.Abstractions;
using OliveOilEshop.Pipelines;
using System.Reflection;

namespace OliveOilEshop.Extensions
{
    public static class HandlerRegistrationExtensions
    {
        public static IServiceCollection AddHandlersFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            var handlerTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && !t.ContainsGenericParameters)
                .ToList();

            foreach (var implementation in handlerTypes)
            {
                var handlerInterfaces = implementation.GetInterfaces().Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IHandler<,>));
                foreach (var handlerInterface in handlerInterfaces)
                {
                    services.AddScoped(handlerInterface, implementation);
                }
            }

            services.Decorate(typeof(IHandler<,>), typeof(ValidationDecorator<,>));
            services.Decorate(typeof(IHandler<,>), typeof(LoggingDecorator<,>));
            services.Decorate(typeof(IHandler<,>), typeof(AuthorizationDecorator<,>));
            return services;
        }
    }
}
