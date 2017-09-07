using System.Threading.Tasks;
using Libraryman.Common.Result;
using Libraryman.DataAccess;
using Libraryman.Entity;

namespace Libraryman.Wpf.Query
{
	public class GetStaffByIdQueryHandler : AsyncQueryHandlerBase<GetStaffById, Result<Staff>>
	{
		public GetStaffByIdQueryHandler(LibrarymanContext context) : base(context)
		{
		}

		public override async Task<Result<Staff>> HandleAsync(GetStaffById query)
		{
			Staff staff = await _context.Staffs.FindAsync(query.Id).ConfigureAwait(false);
			return staff == null
				? Result.Fail<Staff>($"Staff with id {query.Id} not found.")
				: Result.Ok(staff);
		}
	}
}