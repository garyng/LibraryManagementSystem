using System.Threading.Tasks;

namespace Libraryman.Wpf.Command
{
	public interface IAsyncCommandHandler<TCommand, TCommandResult>
		where TCommand : ICommand
	{
		Task<TCommandResult> HandleAsync(TCommand command);
	}
}