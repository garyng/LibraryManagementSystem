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
using MaterialDesignThemes.Wpf;

namespace Libraryman.Wpf.Dashboard
{
	public class DashboardViewModel : ViewModelBase
	{
		public DashboardInfo DashboardInfo { get; set; }
		public RelayCommand LoadDashboardInfoCommand { get; set; }

		public DashboardViewModel(INavigationService<ViewModelBase> navigation, IAsyncCommandDispatcher commandDispatcher,
			IAsyncQueryDispatcher queryDispatcher, ISnackbarMessageQueue snackbarMessageQueue) : base(navigation,
			commandDispatcher, queryDispatcher, snackbarMessageQueue)
		{
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
					.ConfigureAwait(true);
			DashboardInfo.TotalOverdueBooks =
				await _queryDispatcher.DispatchAsync<GetOverdueBookCount, int>(new GetOverdueBookCount())
					.ConfigureAwait(true);
			DashboardInfo.TotalIssuedBooks =
				await _queryDispatcher.DispatchAsync<GetTotalIssuedBookCount, int>(new GetTotalIssuedBookCount())
					.ConfigureAwait(true);
			DashboardInfo.TotalUsers =
				await _queryDispatcher.DispatchAsync<GetTotalUserCount, int>(new GetTotalUserCount())
					.ConfigureAwait(true);
			DashboardInfo.TodayIssueBook =
				await _queryDispatcher.DispatchAsync<GetTodayIssuedBookCount, int>(new GetTodayIssuedBookCount())
					.ConfigureAwait(true);
			DashboardInfo.TodayReturnedBooks =
				await _queryDispatcher.DispatchAsync<GetTodayReturnedBookCount, int>(new GetTodayReturnedBookCount())
					.ConfigureAwait(true);
			DashboardInfo.ThisMonthIssuedBooks =
				await _queryDispatcher.DispatchAsync<GetThisMonthIssuedBookCount, int>(new GetThisMonthIssuedBookCount())
					.ConfigureAwait(true);
			DashboardInfo.ThisMonthReturnedBooks =
				await _queryDispatcher.DispatchAsync<GetThisMonthReturnedBookCount, int>(new GetThisMonthReturnedBookCount())
					.ConfigureAwait(true);

			_snackbarMessageQueue.Enqueue("Loaded new data from database.");
		}
	}
}