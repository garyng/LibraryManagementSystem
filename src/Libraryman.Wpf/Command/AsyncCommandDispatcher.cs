using System.Threading.Tasks;

namespace Libraryman.Wpf.Command
{
	public class AsyncCommandDispatcher : IAsyncCommandDispatcher
	{
		private readonly ICommandQueryResolver _resolver;

		public AsyncCommandDispatcher(ICommandQueryResolver resolver)
		{
			_resolver = resolver;
		}

		public async Task<TCommandResult> DispatchAsync<TCommand, TCommandResult>(TCommand command) where TCommand : ICommand
		{
			var handler = _resolver.Resolve<IAsyncCommandHandler<TCommand, TCommandResult>>();
			return await handler.HandleAsync(command).ConfigureAwait(false);
		}
	}
}