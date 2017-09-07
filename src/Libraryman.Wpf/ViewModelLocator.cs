using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using Autofac;
using GalaSoft.MvvmLight;
using Libraryman.Common.Result;
using Libraryman.DataAccess;
using Libraryman.Entity;
using Libraryman.Wpf.Command;
using Libraryman.Wpf.Login;
using Libraryman.Wpf.Navigation;
using Libraryman.Wpf.Query;
using Libraryman.Wpf.Service;

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

			cb.RegisterType<LibrarymanContext>()
				.AsSelf()
				.SingleInstance();

			cb.RegisterType<CommandQueryResolver>()
				.As<ICommandQueryResolver>()
				.SingleInstance();

			cb.RegisterType<AsyncCommandDispatcher>()
				.As<IAsyncCommandDispatcher>()
				.SingleInstance();
			cb.RegisterType<AsyncQueryDispatcher>()
				.As<IAsyncQueryDispatcher>()
				.SingleInstance();

			cb.RegisterType<GetStaffByIdQueryHandler>()
				.As<IAsyncQueryHandler<GetStaffById, Result<Staff>>>();
			cb.RegisterType<UpdateLastLoginTimeByStaffIdCommandHandler>()
				.As<IAsyncCommandHandler<UpdateLastLoginTimeByStaffId, Result>>();

			cb.RegisterType<AuthenticationService>()
				.As<IAuthenticationService>();

			this.Container = cb.Build();

			this.Container.Resolve<INavigationService<ViewModelBase>>()
				.GoTo<LoginViewModel>();

			//using (var context = new LibrarymanContext())
			//{
			//	context.Database.Initialize(true);
			//}
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