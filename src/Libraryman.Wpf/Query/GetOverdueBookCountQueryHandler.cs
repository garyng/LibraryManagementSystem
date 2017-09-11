using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Libraryman.DataAccess;

namespace Libraryman.Wpf.Query
{
	public class GetOverdueBookCountQueryHandler : AsyncQueryHandlerBase<GetOverdueBookCount, int>
	{
		public GetOverdueBookCountQueryHandler(LibrarymanContext context) : base(context)
		{
		}

		public override async Task<int> HandleAsync(GetOverdueBookCount query)
		{
			return await _context
				.BorrowedBooks
				.AsNoTracking()
				.CountAsync(bb => DateTime.Now > bb.DueDate)
				.ConfigureAwait(false);
		}
	}
}