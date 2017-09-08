using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using Libraryman.Common.Result;
using Libraryman.DataAccess;
using Libraryman.Entity;
using Libraryman.Wpf.Command;
using Libraryman.Wpf.Navigation;
using Libraryman.Wpf.Query;
using Libraryman.Wpf.Service;
using Optional;

namespace Libraryman.Wpf.Return
{
	public class GetBorrowedBookDetailsByBarcode : IQuery
	{
		public int Barcode { get; set; }
	}

	public class GetBorrowedBookDetailsByBarcodeQueryHandler : AsyncQueryHandlerBase<GetBorrowedBookDetailsByBarcode,
		Option<BorrowedBook>>
	{
		public GetBorrowedBookDetailsByBarcodeQueryHandler(LibrarymanContext context) : base(context)
		{
		}

		public override async Task<Option<BorrowedBook>> HandleAsync(GetBorrowedBookDetailsByBarcode query)
		{
			BorrowedBook result = await
				_context
					.BorrowedBooks
					.Include(bb => bb.Record)
					.Include(bb => bb.Book)
					.Include(bb => bb.User)
					.Include(bb => bb.User.Type)
					// .AsNoTracking()
					// should be single or default
					.FirstOrDefaultAsync(bb => bb.BookBarcode == query.Barcode)
					.ConfigureAwait(false);

			return result == null ? Option.None<BorrowedBook>() : Option.Some(result);
		}
	}

	public class BorrowedBookInfo
	{
		public int BookBarcode { get; set; }
		public string BookISBN { get; set; }
		public string BookTitle { get; set; }
		public string BookEdition { get; set; }
		public string PublishedYear { get; set; }

		public int UserId { get; set; }
		public string UserName { get; set; }

		public int RecordId { get; set; }
		public DateTime BorrowedDate { get; set; }
		public int StaffId { get; set; }
		public DateTime DueDate { get; set; }

		public bool IsOverdue { get; set; }
		public decimal OverdueFine { get; set; }

		public BorrowedBookInfo()
		{
		}

		public BorrowedBookInfo(BorrowedBook borrowedBookDetails)
		{
			BookBarcode = borrowedBookDetails.BookBarcode;
			BookISBN = borrowedBookDetails.Book.ISBN;
			BookTitle = borrowedBookDetails.Book.Title;
			BookEdition = borrowedBookDetails.Book.Edition;
			PublishedYear = borrowedBookDetails.Book.PublishedYear;
			UserId = borrowedBookDetails.UserId;
			UserName = borrowedBookDetails.User.Name;
			BorrowedDate = borrowedBookDetails.Record.Timestamp;
			RecordId = borrowedBookDetails.RecordId;
			StaffId = borrowedBookDetails.Record.StaffId;
			DueDate = borrowedBookDetails.DueDate;
			IsOverdue = borrowedBookDetails.DueDate.Date < DateTime.Today;
			OverdueFine = IsOverdue
				? (DateTime.Today - borrowedBookDetails.DueDate.Date).Days * borrowedBookDetails.User.Type.OverdueFine
				: 0;
		}
	}

	public class ReturnBorrowedBookByRecordId : ICommand
	{
		// get record id
		// fill in returned book
		public int RecordId { get; set; }

		public int StaffId { get; set; }
	}

	public class ReturnBorrowedBookByRecordIdCommandHandler : AsyncCommandHandlerBase<ReturnBorrowedBookByRecordId, Result>
	{
		public ReturnBorrowedBookByRecordIdCommandHandler(LibrarymanContext context) : base(context)
		{
		}

		public override async Task<Result> HandleAsync(ReturnBorrowedBookByRecordId command)
		{
			Record record = await _context
				.Records
				.Include(r => r.User)
				.Include(r => r.Staff)
				.Include(r => r.Book)
				.SingleOrDefaultAsync(r => r.Id == command.RecordId)
				.ConfigureAwait(false);
			if (record == null) return Result.Fail($"Record #{command.RecordId} not found!");

			BorrowedBook borrowedBook = await _context
				.BorrowedBooks
				.FindAsync(record.Id, record.UserId, record.BookBarcode)
				.ConfigureAwait(false);

			if (borrowedBook == null)
				return Result.Fail(
					$"Is the book [RecordId: {record.Id} UserId: {record.UserId} BookBarcode: {record.BookBarcode}] issued?");
			bool isOverdue = borrowedBook.DueDate.Date < DateTime.Today;
			decimal overdueFine = isOverdue
				? (DateTime.Today - borrowedBook.DueDate.Date).Days * record.User.Type.OverdueFine
				: 0;

			ReturnedBook returnedBook = new ReturnedBook()
			{
				Record = new Record()
				{
					BookBarcode = record.BookBarcode,
					StaffId = command.StaffId,
					Timestamp = DateTime.Now,
					Type = RecordType.Return,
					UserId = record.UserId
				},
				BookBarcode = record.BookBarcode,
				BorrowingRecordId = record.Id,
				OverdueFine = overdueFine,
				UserId = record.UserId
			};

			_context.BorrowedBooks.Remove(borrowedBook);
			_context.ReturnedBooks.Add(returnedBook);
			await _context.SaveChangesAsync().ConfigureAwait(false);

			return Result.Ok();
		}
	}

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
				if (BarcodeSearchString.Length == 0)
				{
					IsFound = Option.None<bool>();
				}
			}
		}

		private Option<bool> _isFound;

		public Option<bool> IsFound
		{
			get => _isFound;
			set => Set(ref _isFound, value);
		}

		private BorrowedBookInfo _bookSearchResult;

		public BorrowedBookInfo BookSearchResult
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
				BookSearchResult = new BorrowedBookInfo()
				{
					BookBarcode = 123,
					BookISBN = "431-22-11408-34-4",
					BookTitle = "Title",
					BookEdition = "3rd Edition",
					PublishedYear = "2017",
					UserId = 1000002,
					UserName = "Username",
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
					BookSearchResult = new BorrowedBookInfo(bb);
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