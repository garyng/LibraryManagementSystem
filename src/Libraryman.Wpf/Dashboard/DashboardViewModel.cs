using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using Libraryman.DataAccess;
using Libraryman.Entity;
using Libraryman.Wpf.Command;
using Libraryman.Wpf.Navigation;
using Libraryman.Wpf.Query;

namespace Libraryman.Wpf.Dashboard
{
	public class DashboardViewModel : ViewModelBase
	{
		private readonly IAsyncQueryDispatcher _queryDispatcher;
		private readonly IAsyncCommandDispatcher _commandDispatcher;
		public DashboardInfo DashboardInfo { get; set; }
		public RelayCommand LoadDashboardInfoCommand { get; set; }


		public DashboardViewModel(INavigationService<ViewModelBase> navigation, IAsyncQueryDispatcher queryDispatcher,
			IAsyncCommandDispatcher commandDispatcher) : base(navigation)
		{
			_queryDispatcher = queryDispatcher;
			_commandDispatcher = commandDispatcher;

			DashboardInfo = new DashboardInfo();
			LoadDashboardInfoCommand = new RelayCommand(async () => await LoadDashboardInfo().ConfigureAwait(false));
			if (!IsInDesignModeStatic)
			{
				LoadDashboardInfoCommand?.Execute(null);
			}
		}

		private async Task LoadDashboardInfo()
		{
			DashboardInfo.TotalBooks =
				await _queryDispatcher.DispatchAsync<GetTotalBookCount, int>(new GetTotalBookCount())
					.ConfigureAwait(false);
			DashboardInfo.TotalOverdueBooks =
				await _queryDispatcher.DispatchAsync<GetOverdueBookCount, int>(new GetOverdueBookCount())
					.ConfigureAwait(false);
			DashboardInfo.TotalIssuedBooks =
				await _queryDispatcher.DispatchAsync<GetTotalIssuedBookCount, int>(new GetTotalIssuedBookCount())
					.ConfigureAwait(false);
			DashboardInfo.TotalUsers =
				await _queryDispatcher.DispatchAsync<GetTotalUserCount, int>(new GetTotalUserCount())
					.ConfigureAwait(false);
			DashboardInfo.TodayIssueBook =
				await _queryDispatcher.DispatchAsync<GetTodayIssuedBookCount, int>(new GetTodayIssuedBookCount())
					.ConfigureAwait(false);
			DashboardInfo.TodayReturnedBooks =
				await _queryDispatcher.DispatchAsync<GetTodayReturnedBookCount, int>(new GetTodayReturnedBookCount())
					.ConfigureAwait(false);
			DashboardInfo.ThisMonthIssuedBooks =
				await _queryDispatcher.DispatchAsync<GetThisMonthIssuedBookCount, int>(new GetThisMonthIssuedBookCount())
					.ConfigureAwait(false);
			DashboardInfo.ThisMonthReturnedBooks =
				await _queryDispatcher.DispatchAsync<GetThisMonthReturnedBookCount, int>(new GetThisMonthReturnedBookCount())
					.ConfigureAwait(false);
		}
	}
}