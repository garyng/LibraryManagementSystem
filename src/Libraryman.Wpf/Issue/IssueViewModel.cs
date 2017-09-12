using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using Libraryman.Common.Result;
using Libraryman.Entity;
using Libraryman.Wpf.Command;
using Libraryman.Wpf.Dto;
using Libraryman.Wpf.Faker;
using Libraryman.Wpf.Navigation;
using Libraryman.Wpf.Query;
using Libraryman.Wpf.Service;
using Libraryman.Wpf.UserInfo;
using MaterialDesignThemes.Wpf;
using Optional;

namespace Libraryman.Wpf.Issue
{
	public class IssueViewModel : ViewModelBase
	{
		private readonly AuthenticationState _as;

		public EntitySearcher<UserDto> UserSearcher { get; set; }
		public EntitySearcher<BookDto> BookSearcher { get; set; }

		public RelayCommand IssueBookCommand { get; set; }

		public IssueViewModel(INavigationService<ViewModelBase> navigation, IAsyncCommandDispatcher commandDispatcher,
			IAsyncQueryDispatcher queryDispatcher, ISnackbarMessageQueue snackbarMessageQueue,
			AuthenticationState @as) : base(navigation,
			commandDispatcher, queryDispatcher, snackbarMessageQueue)
		{
			_as = @as;
			UserSearcher = new EntitySearcher<UserDto>(async search => await OnSearchUser(search).ConfigureAwait(false),
				onSearched: () => BookSearcher.ClearSearchString());
			BookSearcher = new EntitySearcher<BookDto>(async search => await OnSearchBook(search).ConfigureAwait(false),
				onSearched: () => IssueBookCommand?.RaiseCanExecuteChanged());
			IssueBookCommand = new RelayCommand(async () => await OnIssueBook().ConfigureAwait(false), CanIssueBook);
#if DEBUG
			if (IsInDesignModeStatic)
			{
				UserSearcher.SearchResult = UserDtoFaker.Generate();
				BookSearcher.SearchResult = BookDtoFaker.Generate();
			}
#endif
		}

		private async Task OnIssueBook()
		{
			int userId = UserSearcher.SearchResult.UserId;
			int staffId = _as.StaffId;
			int bookBarcode = BookSearcher.SearchResult.BookBarcode;

			Result result = await _commandDispatcher.DispatchAsync<IssueBookToUser, Result>(
					new IssueBookToUser() {BookBarcode = bookBarcode, StaffId = staffId, UserId = userId})
				.ConfigureAwait(false);
			result
				.OnSuccess(() =>
				{
					BookSearcher.ClearSearchString();
					_navigation.GoBack<UserInfoViewModel>(vm => { vm.LoadDetailsCommand.Execute(null); });
					_snackbarMessageQueue.Enqueue($"Issued book \"{bookBarcode}\" to user \"{userId}\".");
				})
				.OnFailure((string error) =>
				{
					_snackbarMessageQueue.Enqueue($"Failed to issue book \"{bookBarcode}\" to user \"{userId}\".");
					_snackbarMessageQueue.Enqueue($"Error message: {error}");
				});
		}

		private bool CanIssueBook()
		{
			return BookSearcher.SearchResult?.IsBookBorrowable == true &&
			       BookSearcher.SearchResult?.BookStatus == BookStatus.Available;
		}

		private async Task<Option<BookDto>> OnSearchBook(string searchString)
		{
			Option<Book> book = await _queryDispatcher.DispatchAsync<GetBookByBarcode, Option<Book>>(
					new GetBookByBarcode() {Barcode = int.Parse(searchString)})
				.ConfigureAwait(false);

			return book.Map(b => new BookDto(b));
		}

		private async Task<Option<UserDto>> OnSearchUser(string searchString)
		{
			Option<User> option = await _queryDispatcher.DispatchAsync<GetUserDetailsById, Option<User>>(
					new GetUserDetailsById() {UserId = int.Parse(searchString)})
				.ConfigureAwait(false);
			return option.Map(user => new UserDto(user));
		}
	}
}