using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Libraryman.DataAccess;
using Libraryman.Entity;

namespace Libraryman.Wpf.Query
{
	public class GetTodayReturnedBookCountQueryHandler : AsyncQueryHandlerBase<GetTodayReturnedBookCount, int>
	{
		public GetTodayReturnedBookCountQueryHandler(LibrarymanContext context) : base(context)
		{
		}

		public override async Task<int> HandleAsync(GetTodayReturnedBookCount query)
		{
			DateTime today = DateTime.Now.Date;
			List<Record> records = await _context
				.Records
				.Where(r => r.Type == RecordType.Return)
				.ToListAsync()
				.ConfigureAwait(false);
			return records.Count(r => r.Timestamp.Date == today);
		}
	}
}