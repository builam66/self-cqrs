using CQRS.Command;
using CQRS.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace CQRS.Dispatchers
{
    internal class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandDispatcher(IServiceProvider serviceProvider)
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
    }
}
