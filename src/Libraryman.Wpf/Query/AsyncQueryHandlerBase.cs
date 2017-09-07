using System.Threading.Tasks;
using Libraryman.DataAccess;

namespace Libraryman.Wpf.Query
{
	public abstract class AsyncQueryHandlerBase<TQuery, TQueryResult> : CommandQueryHandlerBase,
		IAsyncQueryHandler<TQuery, TQueryResult>
		where TQuery : IQuery
	{
		public AsyncQueryHandlerBase(LibrarymanContext context) : base(context)
		{
		}

		public abstract Task<TQueryResult> HandleAsync(TQuery query);
	}
}