using Libraryman.DataAccess;

namespace Libraryman.Wpf
{
	public abstract class CommandQueryHandlerBase
	{
		protected LibrarymanContext _context;

		protected CommandQueryHandlerBase(LibrarymanContext context)
		{
			_context = context;
		}
	}
}