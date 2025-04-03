using CQRS.Command;
using CQRS.Dispatchers;
using CQRS.Query;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CQRS
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCQRS(this IServiceCollection services, params Assembly[] assemblies)
        {
            // Register the dispatcher
            services.AddScoped<IDispatcher, Dispatcher>();

            // Register handlers from assemblies
            if (assemblies.Length == 0)
            {
                assemblies = new[] { Assembly.GetCallingAssembly() };
            }

            // Register command handlers
            RegisterCommandHandlers(services, assemblies);

            // Register query handlers
            RegisterQueryHandlers(services, assemblies);

            return services;
        }

        public static IServiceCollection AddQuery(this IServiceCollection services, params Assembly[] assemblies)
        {
            // Register the dispatcher
            services.AddScoped<IDispatcher, Dispatcher>();

            // Register handlers from assemblies
            if (assemblies.Length == 0)
            {
                assemblies = new[] { Assembly.GetCallingAssembly() };
            }

            // Register query handlers
            RegisterQueryHandlers(services, assemblies);

            return services;
        }

        public static IServiceCollection AddCommand(this IServiceCollection services, params Assembly[] assemblies)
        {
            // Register the dispatcher
            services.AddScoped<IDispatcher, Dispatcher>();

            // Register handlers from assemblies
            if (assemblies.Length == 0)
            {
                assemblies = new[] { Assembly.GetCallingAssembly() };
            }

            // Register command handlers
            RegisterCommandHandlers(services, assemblies);

            return services;
        }

        private static void RegisterCommandHandlers(IServiceCollection services, Assembly[] assemblies)
        {
            // Register command handlers (without return value)
            var commandHandlerTypes = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .SelectMany(t => t.GetInterfaces(), (type, interfaceType) => new { type, interfaceType })
                .Where(x => x.interfaceType.IsGenericType &&
                            x.interfaceType.GetGenericTypeDefinition() == typeof(ICommandHandler<>))
                .ToList();

            foreach (var handler in commandHandlerTypes)
            {
                var commandType = handler.interfaceType.GetGenericArguments()[0];
                var handlerInterface = typeof(ICommandHandler<>).MakeGenericType(commandType);
                services.AddScoped(handlerInterface, handler.type);
            }

            // Register command handlers (with return value)
            var commandHandlerWithResultTypes = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .SelectMany(t => t.GetInterfaces(), (type, interfaceType) => new { type, interfaceType })
                .Where(x => x.interfaceType.IsGenericType &&
                            x.interfaceType.GetGenericTypeDefinition() == typeof(ICommandHandler<,>))
                .ToList();

            foreach (var handler in commandHandlerWithResultTypes)
            {
                var genericArgs = handler.interfaceType.GetGenericArguments();
                var commandType = genericArgs[0];
                var resultType = genericArgs[1];
                var handlerInterface = typeof(ICommandHandler<,>).MakeGenericType(commandType, resultType);
                services.AddScoped(handlerInterface, handler.type);
            }
        }

        private static void RegisterQueryHandlers(IServiceCollection services, Assembly[] assemblies)
        {
            var queryHandlerTypes = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .SelectMany(t => t.GetInterfaces(), (type, interfaceType) => new { type, interfaceType })
                .Where(x => x.interfaceType.IsGenericType &&
                            x.interfaceType.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))
                .ToList();

            foreach (var handler in queryHandlerTypes)
            {
                var genericArgs = handler.interfaceType.GetGenericArguments();
                var queryType = genericArgs[0];
                var resultType = genericArgs[1];
                var handlerInterface = typeof(IQueryHandler<,>).MakeGenericType(queryType, resultType);
                services.AddScoped(handlerInterface, handler.type);
            }
        }

        public static IServiceCollection AddHandlersFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            RegisterCommandHandlers(services, new[] { assembly });
            RegisterQueryHandlers(services, new[] { assembly });
            return services;
        }
    }
}
