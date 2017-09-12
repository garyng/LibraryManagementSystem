using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using Libraryman.DataAccess;
using Libraryman.Entity;
using Libraryman.Wpf.Command;
using Libraryman.Wpf.Dto;
using Libraryman.Wpf.Extensions;
using Libraryman.Wpf.Faker;
using Libraryman.Wpf.Navigation;
using Libraryman.Wpf.Query;
using MaterialDesignThemes.Wpf;
using MoreLinq;

namespace Libraryman.Wpf.Issue
{
	public class IssueViewModel : ViewModelBase
	{
		public override bool GoBackOnly { get; } = true;

		private UserDto _user;

		public UserDto User
		{
			get => _user;
			set => Set(ref _user, value);
		}

		private ObservableCollection<RecordDto> _records;

		public ObservableCollection<RecordDto> Records
		{
			get => _records;
			set => Set(ref _records, value);
		}

		private ObservableCollection<BorrowedBookDto> _borrowedBooks;

		public ObservableCollection<BorrowedBookDto> BorrowedBooks
		{
			get => _borrowedBooks;
			set => Set(ref _borrowedBooks, value);
		}


		public RelayCommand LoadDetailsCommand { get; set; }
		public RelayCommand GoToAddBookCommand { get; set; }

		public IssueViewModel(INavigationService<ViewModelBase> navigation, IAsyncCommandDispatcher commandDispatcher,
			IAsyncQueryDispatcher queryDispatcher, ISnackbarMessageQueue snackbarMessageQueue) : base(navigation,
			commandDispatcher, queryDispatcher, snackbarMessageQueue)
		{
			LoadDetailsCommand = new RelayCommand(async () => await OnLoadDetails().ConfigureAwait(false));
			GoToAddBookCommand = new RelayCommand(OnGoToAddBook);

			_records = new ObservableCollection<RecordDto>();
#if DEBUG
			if (IsInDesignModeStatic)
			{
				User = new UserDto()
				{
					UserId = 100001,
					Email = "email@email.com",
					InHandBooksCount = 1000,
					MembershipType = "Non Member",
					PhoneNumber = "+60 123 123 123",
					TotalRecordCount = 21234,
					UserGender = "Female",
					UserName = "Testing"
				};

				Records = RecordDtoFaker.Generate(10).ToObservableCollection();
				BorrowedBooks = BorrowedBookDtoFaker.Generate(10).ToObservableCollection();
			}
#endif

		}

		private async Task OnLoadDetails()
		{
			IEnumerable<RecordDto> records = (await _queryDispatcher
					.DispatchAsync<GetAllRecordByUserId, IEnumerable<Record>>(new GetAllRecordByUserId() {UserId = _user.UserId})
					.ConfigureAwait(false))
				.Select(r => new RecordDto(r))
				.OrderByDescending(r => r.Timestamp);
			Records = records.ToObservableCollection();

			IOrderedEnumerable<BorrowedBookDto> borrowedBooks = (await _queryDispatcher
					.DispatchAsync<GetAllBorrowedBooksByUserId, IEnumerable<BorrowedBook>>(
						new GetAllBorrowedBooksByUserId() {UserId = _user.UserId})
					.ConfigureAwait(false))
				.Select(bb => new BorrowedBookDto(bb))
				.OrderBy(bb => bb.DueDate);
			BorrowedBooks = borrowedBooks.ToObservableCollection();
		}

		private void OnGoToAddBook()
		{
			_navigation.GoTo<AddBookViewModel>(vm =>
			{
				vm.UserId = _user.UserId;
				vm.Searcher.ClearSearchString();
			});
		}
	}
}