using System.Security;
using System.Threading.Tasks;
using Libraryman.Common.Extensions;
using Libraryman.Common.Result;
using Libraryman.Entity;
using Libraryman.Wpf.Command;
using Libraryman.Wpf.Extensions;
using Libraryman.Wpf.Query;

namespace Libraryman.Wpf.Service
{
	public class AuthenticationService : IAuthenticationService
	{
		private readonly IAsyncCommandDispatcher _commandDispatcher;
		private readonly IAsyncQueryDispatcher _queryDispatcher;

		public AuthenticationService(IAsyncCommandDispatcher commandDispatcher, IAsyncQueryDispatcher queryDispatcher)
		{
			_commandDispatcher = commandDispatcher;
			_queryDispatcher = queryDispatcher;
		}


		public async Task<Result<bool>> AuthenticateAsync(int staffId, SecureString password)
		{
			Result<Staff> getStaffResult = await _queryDispatcher
				.DispatchAsync<GetStaffById, Result<Staff>>(new GetStaffById() {Id = staffId})
				.ConfigureAwait(false);

			if (getStaffResult.IsSuccess)
			{
				Staff staff = getStaffResult.Value;
				if (staff.PasswordHash == password.ConvertToString().ToSHA256())
				{
					await _commandDispatcher.DispatchAsync<UpdateLastLoginTimeByStaffId, Result>(
							new UpdateLastLoginTimeByStaffId() {Id = staffId})
						.ConfigureAwait(false);
					return Result.Ok(true);
				}
				return Result.Fail<bool>("Wrong password!");
			}
			return Result.Fail<bool>(getStaffResult);
		}
	}
}