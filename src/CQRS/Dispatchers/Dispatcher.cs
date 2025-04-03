using CQRS.Command;
using CQRS.Exceptions;
using CQRS.Query;
using Microsoft.Extensions.DependencyInjection;

namespace CQRS.Dispatchers
{
    public class Dispatcher : IDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public Dispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
            where TCommand : ICommand
        {
            using var scope = _serviceProvider.CreateScope();
            var handler = scope.ServiceProvider.GetService<ICommandHandler<TCommand>>();

            if (handler == null)
                throw new NotFoundException(typeof(TCommand));

            await handler.HandleAsync(command, cancellationToken);
        }

        public async Task<TResult> SendAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default)
            where TCommand : ICommand<TResult>
        {
            using var scope = _serviceProvider.CreateScope();
            var handler = scope.ServiceProvider.GetService<ICommandHandler<TCommand, TResult>>();

            if (handler == null)
                throw new NotFoundException(typeof(TCommand));

            return await handler.HandleAsync(command, cancellationToken);
        }

        public async Task<TResult> QueryAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default)
            where TQuery : IQuery<TResult>
        {
            using var scope = _serviceProvider.CreateScope();
            var handler = scope.ServiceProvider.GetService<IQueryHandler<TQuery, TResult>>();

            if (handler == null)
                throw new NotFoundException(typeof(TQuery));

            return await handler.HandleAsync(query, cancellationToken);
        }
    }

    public interface IDispatcher : ICommandDispatcher, IQueryDispatcher
    {
    }
}
