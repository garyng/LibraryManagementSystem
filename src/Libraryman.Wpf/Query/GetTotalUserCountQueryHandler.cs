using System.Data.Entity;
using System.Threading.Tasks;
using Libraryman.DataAccess;

namespace Libraryman.Wpf.Query
{
	public class GetTotalUserCountQueryHandler : AsyncQueryHandlerBase<GetTotalUserCount, int>
	{
		public GetTotalUserCountQueryHandler(LibrarymanContext context) : base(context)
		{
		}

		public override async Task<int> HandleAsync(GetTotalUserCount query)
		{
			return await _context
				.Users
				.AsNoTracking()
				.CountAsync()
				.ConfigureAwait(false);
		}
	}
}