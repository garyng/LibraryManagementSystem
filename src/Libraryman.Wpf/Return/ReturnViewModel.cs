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
using Optional;

namespace Libraryman.Wpf.Return
{
	public class ReturnViewModel : ViewModelBase
	{
		private readonly AuthenticationState _as;
		private readonly IAsyncQueryDispatcher _queryDispatcher;
		private readonly IAsyncCommandDispatcher _commandDispatcher;

		private string _barcodeSearchString;

		public string BarcodeSearchString
		{
			get => _barcodeSearchString;
			set
			{
				Set(ref _barcodeSearchString, value);
				SearchCommand?.RaiseCanExecuteChanged();
				// if the search string change, reset to none to hide either the error message view/the result view
				IsFound = Option.None<bool>();
			}
		}

		private Option<bool> _isFound;

		public Option<bool> IsFound
		{
			get => _isFound;
			set => Set(ref _isFound, value);
		}

		private BorrowedBookDto _bookSearchResult;

		public BorrowedBookDto BookSearchResult
		{
			get => _bookSearchResult;
			set => Set(ref _bookSearchResult, value);
		}


		public RelayCommand SearchCommand { get; set; }
		public RelayCommand ReturnBookCommand { get; set; }

		public ReturnViewModel(INavigationService<ViewModelBase> navigation, AuthenticationState @as,
			IAsyncQueryDispatcher queryDispatcher, IAsyncCommandDispatcher commandDispatcher) : base(navigation)
		{
			_as = @as;
			_queryDispatcher = queryDispatcher;
			_commandDispatcher = commandDispatcher;
			SearchCommand = new RelayCommand(async () => await OnSearch().ConfigureAwait(false), CanSearch);
			ReturnBookCommand = new RelayCommand(async () => await OnReturnBook().ConfigureAwait(false));
			_isFound = Option.None<bool>();
#if DEBUG
			if (IsInDesignModeStatic)
			{
				BookSearchResult = new BorrowedBookDto()
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

		private bool CanSearch()
		{
			return _barcodeSearchString?.Length > 0 && int.TryParse(_barcodeSearchString, out int _);
		}

		private async Task OnSearch()
		{
			Option<BorrowedBook> result = await _queryDispatcher
				.DispatchAsync<GetBorrowedBookDetailsByBarcode, Option<BorrowedBook>>(
					new GetBorrowedBookDetailsByBarcode() {Barcode = int.Parse(_barcodeSearchString)})
				.ConfigureAwait(false);

			IsFound = result.Match(
				some: bb =>
				{
					BookSearchResult = new BorrowedBookDto(bb);
					return Option.Some(true);
				},
				none: () => Option.Some(false));
		}

		private async Task OnReturnBook()
		{
			await _commandDispatcher.DispatchAsync<ReturnBorrowedBookByRecordId, Result>(new ReturnBorrowedBookByRecordId()
				{
					RecordId = BookSearchResult.RecordId,
					StaffId = _as.StaffId
				})
				.ConfigureAwait(false);
			BarcodeSearchString = "";
		}
	}
}