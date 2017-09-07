using GalaSoft.MvvmLight;

namespace Libraryman.Wpf.Dashboard
{
	public class DashboardInfo : ObservableObject
	{
		private int _totalBooks;

		public int TotalBooks
		{
			get => _totalBooks;
			set => Set(ref _totalBooks, value);
		}

		private int _totalOverdueBooks;

		public int TotalOverdueBooks
		{
			get => _totalOverdueBooks;
			set => Set(ref _totalOverdueBooks, value);
		}

		private int _totalIssuedBooks;

		public int TotalIssuedBooks
		{
			get => _totalIssuedBooks;
			set => Set(ref _totalIssuedBooks, value);
		}

		private int _totalUsers;

		public int TotalUsers
		{
			get => _totalUsers;
			set => Set(ref _totalUsers, value);
		}


		private int _todayIssueBook;

		public int TodayIssueBook
		{
			get => _todayIssueBook;
			set => Set(ref _todayIssueBook, value);
		}

		private int _todayReturnedBooks;

		public int TodayReturnedBooks
		{
			get => _todayReturnedBooks;
			set => Set(ref _todayReturnedBooks, value);
		}


		private int _thisMonthIssuedBooks;

		public int ThisMonthIssuedBooks
		{
			get => _thisMonthIssuedBooks;
			set => Set(ref _thisMonthIssuedBooks, value);
		}

		private int _thisMonthReturnedBooks;

		public int ThisMonthReturnedBooks
		{
			get => _thisMonthReturnedBooks;
			set => Set(ref _thisMonthReturnedBooks, value);
		}

		public DashboardInfo()
		{
#if DEBUG
			if (GalaSoft.MvvmLight.ViewModelBase.IsInDesignModeStatic)
			{
				TotalBooks = 1234;
				TotalOverdueBooks = 23;
				TotalIssuedBooks = 1320;
				TotalUsers = 100;
				TodayIssueBook = 10;
				TodayReturnedBooks = 4;
				ThisMonthIssuedBooks = 234;
				ThisMonthReturnedBooks = 1234;
			}
#endif
		}
	}
}