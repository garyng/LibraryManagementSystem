using System.Security;
using System.Threading.Tasks;
using Libraryman.Common.Result;

namespace Libraryman.Wpf.Service
{
	public interface IAuthenticationService
	{
		Task<Result<bool>> AuthenticateAsync(int staffId, SecureString password);
	}
}