using System.Threading.Tasks;
using Libraryman.Common.Result;
using Libraryman.DataAccess;
using Libraryman.Entity;

namespace Libraryman.Wpf.Command
{
	public class UpdateLastLoginTimeByStaffIdCommandHandler : AsyncCommandHandlerBase<UpdateLastLoginTimeByStaffId, Result>
	{
		public UpdateLastLoginTimeByStaffIdCommandHandler(LibrarymanContext context) : base(context)
		{
		}

		public override async Task<Result> HandleAsync(UpdateLastLoginTimeByStaffId command)
		{
			Staff staff = await _context.Staffs.FindAsync(command.Id).ConfigureAwait(false);
			staff.LastLogin = command.LastLogin;
			await _context.SaveChangesAsync().ConfigureAwait(false);
			return Result.Ok();
		}
	}
}