using System.Threading.Tasks;

namespace Libraryman.Wpf.Query
{
	public class AsyncQueryDispatcher : IAsyncQueryDispatcher
	{
		private readonly ICommandQueryResolver _resolver;

		public AsyncQueryDispatcher(ICommandQueryResolver resolver)
		{
			_resolver = resolver;
		}

		public async Task<TQueryResult> DispatchAsync<TQuery, TQueryResult>(TQuery query) where TQuery : IQuery
		{
			var handler = _resolver.Resolve<IAsyncQueryHandler<TQuery, TQueryResult>>();
			return await handler.HandleAsync(query).ConfigureAwait(false);
		}
	
	}
}