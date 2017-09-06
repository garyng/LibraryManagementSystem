using System.Threading.Tasks;

namespace Libraryman.Wpf.Query
{
	public interface IAsyncQueryHandler<TQuery, TQueryResult> where TQuery : IQuery
	{
		Task<TQueryResult> HandleAsync(TQuery query);
	}
}