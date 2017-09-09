using System.Data.Entity;
using System.Threading.Tasks;
using Libraryman.DataAccess;
using Libraryman.Entity;
using Optional;

namespace Libraryman.Wpf.Query
{
	public class GetBorrowedBookDetailsByBarcodeQueryHandler : AsyncQueryHandlerBase<GetBorrowedBookDetailsByBarcode,
		Option<BorrowedBook>>
	{
		public GetBorrowedBookDetailsByBarcodeQueryHandler(LibrarymanContext context) : base(context)
		{
		}

		public override async Task<Option<BorrowedBook>> HandleAsync(GetBorrowedBookDetailsByBarcode query)
		{
			BorrowedBook result = await
				_context
					.BorrowedBooks
					.Include(bb => bb.Record)
					.Include(bb => bb.Book)
					.Include(bb => bb.User)
					.Include(bb => bb.User.Type)
					// .AsNoTracking()
					// should be single or default
					.FirstOrDefaultAsync(bb => bb.BookBarcode == query.Barcode)
					.ConfigureAwait(false);
			return result.SomeNotNull();
		}
	}
}