using System.Threading.Tasks;

namespace Libraryman.Wpf.Query
{
	public interface IAsyncQueryDispatcher
	{
		Task<TQueryResult> DispatchAsync<TQuery, TQueryResult>(TQuery query) where TQuery : IQuery;
	}
}