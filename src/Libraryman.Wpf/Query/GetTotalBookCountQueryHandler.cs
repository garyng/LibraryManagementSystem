using System.Data.Entity;
using System.Threading.Tasks;
using Libraryman.DataAccess;

namespace Libraryman.Wpf.Query
{
	public class GetTotalBookCountQueryHandler : AsyncQueryHandlerBase<GetTotalBookCount, int>
	{
		public GetTotalBookCountQueryHandler(LibrarymanContext context) : base(context)
		{
		}

		public override async Task<int> HandleAsync(GetTotalBookCount query)
		{
			return await _context
				.Books
				.AsNoTracking()
				.CountAsync()
				.ConfigureAwait(false);
		}
	}
}