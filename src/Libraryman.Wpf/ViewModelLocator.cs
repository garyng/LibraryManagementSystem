using Autofac;

namespace Libraryman.Wpf
{
	public class ViewModelLocator
	{
		public IContainer Container { get; }
		public ShellViewModel ShellViewModel => this.Container.Resolve<ShellViewModel>();


		public ViewModelLocator()
		{
			var cb = new ContainerBuilder();
			cb.RegisterType<ShellViewModel>()
				.AsSelf();

			this.Container = cb.Build();
		}
	}
}