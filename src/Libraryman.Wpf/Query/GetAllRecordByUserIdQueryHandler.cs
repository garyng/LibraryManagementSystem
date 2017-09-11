using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Libraryman.DataAccess;
using Libraryman.Entity;

namespace Libraryman.Wpf.Query
{
	public class GetAllRecordByUserIdQueryHandler : AsyncQueryHandlerBase<GetAllRecordByUserId, IEnumerable<Record>>
	{
		public GetAllRecordByUserIdQueryHandler(LibrarymanContext context) : base(context)
		{
		}

		public override async Task<IEnumerable<Record>> HandleAsync(GetAllRecordByUserId query)
		{
			List<Record> records = await _context
				.Records
				.AsNoTracking()
				.Include(r => r.Staff)
				.Include(r => r.Book)
				.Include(r => r.Book.Type)
				.Include(r => r.Book.Publisher)
				.Include(r => r.Book.Authors.Select(a => a.Author))
				.Where(r => r.UserId == query.UserId)
				.ToListAsync()
				.ConfigureAwait(false);
			return records?.Count > 0 ? records : new List<Record>();
		}
	}
}