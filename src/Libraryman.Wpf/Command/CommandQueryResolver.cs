using Autofac;

namespace Libraryman.Wpf.Command
{
	public class CommandQueryResolver : ICommandQueryResolver
	{
		private readonly IComponentContext _container;

		public CommandQueryResolver(IComponentContext container)
		{
			_container = container;
		}

		public T Resolve<T>()
		{
			return _container.Resolve<T>();
		}
	}
}