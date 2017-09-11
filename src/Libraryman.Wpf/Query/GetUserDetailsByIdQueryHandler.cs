using System.Data.Entity;
using System.Threading.Tasks;
using Libraryman.DataAccess;
using Libraryman.Entity;
using Optional;

namespace Libraryman.Wpf.Query
{
	public class GetUserDetailsByIdQueryHandler : AsyncQueryHandlerBase<GetUserDetailsById, Option<User>>
	{
		public GetUserDetailsByIdQueryHandler(LibrarymanContext context) : base(context)
		{
		}

		public override async Task<Option<User>> HandleAsync(GetUserDetailsById query)
		{
			User user = await _context
				.Users
				.AsNoTracking()
				.Include(u => u.Type)
				.Include(u => u.Records)
				.Include(u => u.BorrowedBooks)
				.SingleOrDefaultAsync(u => u.Id == query.UserId)
				.ConfigureAwait(false);
			return user.SomeNotNull();
		}
	}
}