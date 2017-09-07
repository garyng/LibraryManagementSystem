using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Libraryman.DataAccess;
using Libraryman.Entity;

namespace Libraryman.Wpf.Query
{
	public class GetThisMonthReturnedBookCountQueryHandler : AsyncQueryHandlerBase<GetThisMonthReturnedBookCount, int>
	{
		public GetThisMonthReturnedBookCountQueryHandler(LibrarymanContext context) : base(context)
		{
		}

		public override async Task<int> HandleAsync(GetThisMonthReturnedBookCount query)
		{
			DateTime now = DateTime.Now;
			List<Record> records = await _context
				.Records
				.Where(r => r.Type == RecordType.Return)
				.ToListAsync()
				.ConfigureAwait(false);

			return records.Count(r => r.Timestamp.Date.Year == now.Year && r.Timestamp.Date.Month == now.Month);
		}
	}
}