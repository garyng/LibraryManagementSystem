using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Libraryman.DataAccess;
using Libraryman.Entity;

namespace Libraryman.Wpf.Query
{
	public class GetAllBorrowedBooksByUserIdQueryHandler : AsyncQueryHandlerBase<GetAllBorrowedBooksByUserId,
		IEnumerable<BorrowedBook>>
	{
		public GetAllBorrowedBooksByUserIdQueryHandler(LibrarymanContext context) : base(context)
		{
		}

		public override async Task<IEnumerable<BorrowedBook>> HandleAsync(GetAllBorrowedBooksByUserId query)
		{
			List<BorrowedBook> borrowedBooks = await _context
				.BorrowedBooks
				.AsNoTracking()
				.Include(bb => bb.Record)
				.Include(bb => bb.Book)
				.Include(bb => bb.User)
				.Include(bb => bb.User.Type)
				.Where(bb => bb.UserId == query.UserId)
				.ToListAsync()
				.ConfigureAwait(false);

			return borrowedBooks?.Count > 0 ? borrowedBooks : new List<BorrowedBook>();
		}
	}
}