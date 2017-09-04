using System.Collections;
using System.Collections.Generic;
using Autofac;
using GalaSoft.MvvmLight;
using Libraryman.Wpf.Login;
using Libraryman.Wpf.Navigation;

namespace Libraryman.Wpf
{
	public class ViewModelLocator
	{
		public IContainer Container { get; }
		public ShellViewModel ShellViewModel => this.Container.Resolve<ShellViewModel>();
		public LoginViewModel LoginViewModel => this.Container.Resolve<LoginViewModel>();

		public ViewModelLocator()
		{
			var cb = new ContainerBuilder();
			cb.RegisterType<ShellViewModel>()
				.As<INavigationHost<ViewModelBase>>()
				.AsSelf()
				.SingleInstance();

			cb.RegisterType<NavigationService<ViewModelBase>>()
				.As<INavigationService<ViewModelBase>>()
				.SingleInstance();

			cb.RegisterType<LoginViewModel>()
				.AsSelf()
				.As<ViewModelBase>()
				.SingleInstance();

			cb.RegisterType<NavigationTargetsRegistration>()
				.AsSelf()
				.AutoActivate();

			this.Container = cb.Build();

			this.Container.Resolve<INavigationService<ViewModelBase>>()
				.GoTo<LoginViewModel>();
		}
	}

	public class NavigationTargetsRegistration
	{
		public NavigationTargetsRegistration(INavigationService<ViewModelBase> navigation,
			IEnumerable<ViewModelBase> viewModels)
		{
			foreach (ViewModelBase viewModel in viewModels)
			{
				navigation.Register(viewModel);
			}
		}
	}
}