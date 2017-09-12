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
using MaterialDesignThemes.Wpf;
using Optional;

namespace Libraryman.Wpf.UserInfo
{
	public class AddBookViewModel : ViewModelBase
	{
		private readonly AuthenticationState _as;
		public override bool GoBackOnly { get; } = true;

		public int UserId { get; set; }

		public EntitySearcher<BookDto> Searcher { get; set; }
		public RelayCommand AddBookCommand { get; set; }

		public AddBookViewModel(INavigationService<ViewModelBase> navigation, IAsyncCommandDispatcher commandDispatcher,
			IAsyncQueryDispatcher queryDispatcher, ISnackbarMessageQueue snackbarMessageQueue, AuthenticationState @as) : base(
			navigation, commandDispatcher, queryDispatcher, snackbarMessageQueue)
		{
			_as = @as;
			Searcher = new EntitySearcher<BookDto>(async search => await OnSearch(search).ConfigureAwait(false),
				onSearched: () => AddBookCommand?.RaiseCanExecuteChanged());
			AddBookCommand = new RelayCommand(async () => await OnAddBook().ConfigureAwait(false), CanAddBook);
#if DEBUG
			if (IsInDesignModeStatic)
			{
				Searcher.SearchResult = BookDtoFaker.Generate();
			}
#endif
		}

		private async Task OnAddBook()
		{
			int userId = UserId;
			int staffId = _as.StaffId;
			int bookBarcode = Searcher.SearchResult.BookBarcode;

			Result result = await _commandDispatcher.DispatchAsync<IssueBookToUser, Result>(
					new IssueBookToUser() {BookBarcode = bookBarcode, StaffId = staffId, UserId = userId})
				.ConfigureAwait(false);
			result
				.OnSuccess(() =>
				{
					Searcher.ClearSearchString();
					_navigation.GoBack<UserInfoViewModel>(vm => { vm.LoadDetailsCommand.Execute(null); });
					_snackbarMessageQueue.Enqueue($"Issued book \"{bookBarcode}\" to user \"{userId}\".");
				})
				.OnFailure((string error) =>
				{
					_snackbarMessageQueue.Enqueue($"Failed to issue book \"{bookBarcode}\" to user \"{userId}\".");
					_snackbarMessageQueue.Enqueue($"Error message: {error}");
				});
		}

		private bool CanAddBook()
		{
			return Searcher.SearchResult?.IsBookBorrowable == true && Searcher.SearchResult?.BookStatus == BookStatus.Available;
		}

		private async Task<Option<BookDto>> OnSearch(string search)
		{
			Option<Book> book = await _queryDispatcher.DispatchAsync<GetBookByBarcode, Option<Book>>(
					new GetBookByBarcode() {Barcode = int.Parse(search)})
				.ConfigureAwait(false);

			return book.Map(b => new BookDto(b));
		}
	}
}