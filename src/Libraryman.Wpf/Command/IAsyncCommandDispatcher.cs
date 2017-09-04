using System.Threading.Tasks;

namespace Libraryman.Wpf.Command
{
	public interface IAsyncCommandDispatcher
	{
		Task<TCommandResult> DispatchAsync<TCommand, TCommandResult>(TCommand command) where TCommand : ICommand;
	}
}