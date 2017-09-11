using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Reflection;
using Autofac;
using Autofac.Features.AttributeFilters;
using Libraryman.Common.Result;
using Libraryman.DataAccess;
using Libraryman.Entity;
using Libraryman.Wpf.Command;
using Libraryman.Wpf.Dashboard;
using Libraryman.Wpf.Extensions;
using Libraryman.Wpf.Issue;
using Libraryman.Wpf.Login;
using Libraryman.Wpf.Main;
using Libraryman.Wpf.Navigation;
using Libraryman.Wpf.Query;
using Libraryman.Wpf.Return;
using Libraryman.Wpf.Service;
using Libraryman.Wpf.Shell;
using Optional;

namespace Libraryman.Wpf
{
	public class ViewModelLocator
	{
		public IContainer Container { get; }

		public MainViewModel MainViewModel => this.Container.Resolve<MainViewModel>();

		public ShellHeaderViewModel ShellHeaderViewModel => this.Container.Resolve<ShellHeaderViewModel>();
		public ShellMainMenuViewModel ShellMainMenuViewModel => this.Container.Resolve<ShellMainMenuViewModel>();

		public LoginViewModel LoginViewModel => this.Container.Resolve<LoginViewModel>();
		public DashboardViewModel DashboardViewModel => this.Container.Resolve<DashboardViewModel>();
		public ReturnViewModel ReturnViewModel => this.Container.Resolve<ReturnViewModel>();
		public SearchUserViewModel SearchUserViewModel => this.Container.Resolve<SearchUserViewModel>();
		public IssueViewModel IssueViewModel => this.Container.Resolve<IssueViewModel>();

		public ViewModelLocator()
		{
			var cb = new ContainerBuilder();

			cb.RegisterType<NavigationService<ViewModelBase>>()
				.As<INavigationService<ViewModelBase>>()
				.SingleInstance();

			cb.RegisterType<MainViewModel>()
				.As<INavigationHost<ViewModelBase>>()
				.AsSelf()
				.SingleInstance();

			cb.RegisterType<AuthenticationState>()
				.AsSelf()
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

			Assembly asm = Assembly.GetExecutingAssembly();
			cb.RegisterAssemblyTypes(asm)
				.AsClosedTypesOf(typeof(IAsyncQueryHandler<,>))
				.AsImplementedInterfaces();
			cb.RegisterAssemblyTypes(asm)
				.AsClosedTypesOf(typeof(IAsyncCommandHandler<,>))
				.AsImplementedInterfaces();
			cb.RegisterAssemblyTypes(asm)
				.AssignableTo<INavigationTarget>()
				.AsSelf()
				.As<ViewModelBase>()
				.SingleInstance();

			cb.RegisterType<AuthenticationService>()
				.As<IAuthenticationService>();

			cb.RegisterType<AutomateGui>()
				.AsSelf();

			this.Container = cb.Build();

			//this.Container.Resolve<INavigationService<ViewModelBase>>()
			//	// .GoTo<SearchUserViewModel>();
			//	.GoTo<LoginViewModel>();

			if (!GalaSoft.MvvmLight.ViewModelBase.IsInDesignModeStatic)
			{
				this.Container.Resolve<AutomateGui>().Automate();

			}

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

	public class AutomateGui
	{
		private readonly INavigationService<ViewModelBase> _navigation;
		private readonly LoginViewModel _login;
		private readonly SearchUserViewModel _searchUser;

		public AutomateGui(LoginViewModel login, SearchUserViewModel searchUser)
		{
			_login = login;
			_searchUser = searchUser;
		}

		public void Automate()
		{
			_login.StaffId = "1010";
			_login.StaffPassword = "garyng".ConvertFromString();
			
			_login.LoginCommand.Execute(null);

			_searchUser.SearchString = "1000000";
			_searchUser.SearchCommand.Execute(null);
			_searchUser.IssueBookCommand.Execute(null);

		}
	}
}