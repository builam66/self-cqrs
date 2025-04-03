using CQRS.Exceptions;
using CQRS.Query;
using Microsoft.Extensions.DependencyInjection;

namespace CQRS.Dispatchers
{
    internal class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public QueryDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
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
}
