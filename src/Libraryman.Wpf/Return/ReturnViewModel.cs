using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using Libraryman.Common.Result;
using Libraryman.DataAccess;
using Libraryman.Entity;
using Libraryman.Wpf.Command;
using Libraryman.Wpf.Dto;
using Libraryman.Wpf.Navigation;
using Libraryman.Wpf.Query;
using Libraryman.Wpf.Service;
using MaterialDesignThemes.Wpf;
using Optional;

namespace Libraryman.Wpf.Return
{
	public class ReturnViewModel : ViewModelBase
	{
		private readonly AuthenticationState _as;

		public RelayCommand ReturnBookCommand { get; set; }
		public EntitySearcher<BorrowedBookDto> Searcher { get; set; }

		public ReturnViewModel(INavigationService<ViewModelBase> navigation, IAsyncCommandDispatcher commandDispatcher,
			IAsyncQueryDispatcher queryDispatcher, ISnackbarMessageQueue snackbarMessageQueue, AuthenticationState @as) : base(
			navigation, commandDispatcher, queryDispatcher, snackbarMessageQueue)
		{
			_as = @as;
			Searcher = new EntitySearcher<BorrowedBookDto>(async search => await OnSearch(search).ConfigureAwait(false));
			ReturnBookCommand = new RelayCommand(async () => await OnReturnBook().ConfigureAwait(false));
#if DEBUG
			if (IsInDesignModeStatic)
			{
				Searcher.SearchResult = new BorrowedBookDto()
				{
					BookBarcode = 123,
					BookISBN = "431-22-11408-34-4",
					BookTitle = "Title",
					BookEdition = "3rd Edition",
					PublishedYear = "2017",
					UserId = 1000002,
					UserName = "Username",
					UserType = "Member",
					BorrowedDate = DateTime.Today,
					RecordId = 1,
					StaffId = 1001,
					DueDate = DateTime.Today,
					IsOverdue = true,
					OverdueFine = 10.21M
				};
			}
#endif
		}

		private async Task<Option<BorrowedBookDto>> OnSearch(string searchString)
		{
			Option<BorrowedBook> result = await _queryDispatcher
				.DispatchAsync<GetBorrowedBookDetailsByBarcode, Option<BorrowedBook>>(
					new GetBorrowedBookDetailsByBarcode() {Barcode = int.Parse(searchString)})
				.ConfigureAwait(false);
			return result.Map(bb => new BorrowedBookDto(bb));
		}

		private async Task OnReturnBook()
		{
			Result result = await _commandDispatcher.DispatchAsync<ReturnBorrowedBookByRecordId, Result>(
					new ReturnBorrowedBookByRecordId()
					{
						RecordId = Searcher.SearchResult.RecordId,
						StaffId = _as.StaffId
					})
				.ConfigureAwait(false);
			result.OnSuccess(() =>
				{
					Searcher.ClearSearchString();
					_snackbarMessageQueue.Enqueue("Book returned.");
				})
				.OnFailure((string error) =>
				{
					_snackbarMessageQueue.Enqueue("Failed to return book.");
					_snackbarMessageQueue.Enqueue($"Error message: {error}");
				});
		}
	}
}