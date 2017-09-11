using System.Data.Entity;
using System.Threading.Tasks;
using Libraryman.DataAccess;
using Libraryman.Entity;

namespace Libraryman.Wpf.Query
{
	public class GetTotalIssuedBookCountQueryHandler : AsyncQueryHandlerBase<GetTotalIssuedBookCount, int>
	{
		public GetTotalIssuedBookCountQueryHandler(LibrarymanContext context) : base(context)
		{
		}

		public override async Task<int> HandleAsync(GetTotalIssuedBookCount query)
		{
			return await _context
				.Records
				.AsNoTracking()
				.CountAsync(r => r.Type == RecordType.Issue)
				.ConfigureAwait(false);
		}
	}
}