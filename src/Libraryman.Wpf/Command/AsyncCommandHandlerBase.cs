using System.Threading.Tasks;
using Libraryman.DataAccess;

namespace Libraryman.Wpf.Command
{
	public abstract class AsyncCommandHandlerBase<TCommand, TCommandResult> : CommandQueryHandlerBase,
		IAsyncCommandHandler<TCommand, TCommandResult>
		where TCommand : ICommand
	{
		public AsyncCommandHandlerBase(LibrarymanContext context) : base(context)
		{
		}

		public abstract Task<TCommandResult> HandleAsync(TCommand command);
	}
}