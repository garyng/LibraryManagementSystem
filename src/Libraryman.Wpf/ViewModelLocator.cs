﻿using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using Autofac;
using Libraryman.Common.Result;
using Libraryman.DataAccess;
using Libraryman.Entity;
using Libraryman.Wpf.Command;
using Libraryman.Wpf.Dashboard;
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
		public DashboardViewModel DashboardViewModel => this.Container.Resolve<DashboardViewModel>();

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

			cb.RegisterType<AuthenticationState>()
				.AsSelf()
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
			cb.RegisterType<GetTotalBookCountQueryHandler>()
				.As<IAsyncQueryHandler<GetTotalBookCount, int>>();
			cb.RegisterType<GetOverdueBookCountQueryHandler>()
				.As<IAsyncQueryHandler<GetOverdueBookCount, int>>();
			cb.RegisterType<GetTotalIssuedBookCountQueryHandler>()
				.As<IAsyncQueryHandler<GetTotalIssuedBookCount, int>>();
			cb.RegisterType<GetTotalUserCountQueryHandler>()
				.As<IAsyncQueryHandler<GetTotalUserCount, int>>();
			cb.RegisterType<GetTodayIssuedBookCountQueryHandler>()
				.As<IAsyncQueryHandler<GetTodayIssuedBookCount, int>>();
			cb.RegisterType<GetTodayReturnedBookCountQueryHandler>()
				.As<IAsyncQueryHandler<GetTodayReturnedBookCount, int>>();
			cb.RegisterType<GetThisMonthIssuedBookCountQueryHandler>()
				.As<IAsyncQueryHandler<GetThisMonthIssuedBookCount, int>>();
			cb.RegisterType<GetThisMonthReturnedBookCountQueryHandler>()
				.As<IAsyncQueryHandler<GetThisMonthReturnedBookCount, int>>();

			cb.RegisterType<UpdateLastLoginTimeByStaffIdCommandHandler>()
				.As<IAsyncCommandHandler<UpdateLastLoginTimeByStaffId, Result>>();

			cb.RegisterType<AuthenticationService>()
				.As<IAuthenticationService>();

			cb.RegisterType<DashboardViewModel>()
				.AsSelf()
				.As<ViewModelBase>()
				.SingleInstance();

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