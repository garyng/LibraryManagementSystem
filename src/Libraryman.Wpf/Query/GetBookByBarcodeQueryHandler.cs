using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Libraryman.DataAccess;
using Libraryman.Entity;
using Optional;

namespace Libraryman.Wpf.Query
{
	public class GetBookByBarcodeQueryHandler : AsyncQueryHandlerBase<GetBookByBarcode, Option<Book>>
	{
		public GetBookByBarcodeQueryHandler(LibrarymanContext context) : base(context)
		{
		}

		public override async Task<Option<Book>> HandleAsync(GetBookByBarcode query)
		{
			Book book = await _context
				.Books
				.AsNoTracking()
				.Include(b => b.Type)
				.Include(b => b.Publisher)
				.Include(b => b.Publisher)
				.Include(b => b.Authors.Select(a => a.Author))
				.Include(b => b.Library)
				.SingleOrDefaultAsync(b => b.Barcode == query.Barcode)
				.ConfigureAwait(false);
			return book.SomeNotNull();
		}
	}
}