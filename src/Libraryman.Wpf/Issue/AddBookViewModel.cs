using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using Libraryman.Common.Result;
using Libraryman.DataAccess;
using Libraryman.Entity;
using Libraryman.Wpf.Command;
using Libraryman.Wpf.Faker;
using Libraryman.Wpf.Navigation;
using Libraryman.Wpf.Query;
using Libraryman.Wpf.Service;
using MaterialDesignThemes.Wpf;
using Optional;

namespace Libraryman.Wpf.Issue
{
	public class BookDto
	{
		public int BookBarcode { get; set; }
		public string BookISBN { get; set; }
		public string BookTitle { get; set; }
		public string BookEdition { get; set; }
		public decimal BookPrice { get; set; }
		public string BookDescription { get; set; }
		public string BookPublishedYear { get; set; }
		public BookStatus BookStatus { get; set; }
		public string BookType { get; set; }
		public bool IsBookBorrowable { get; set; }
		public string PublisherName { get; set; }
		public string LibraryName { get; set; }
		public IEnumerable<string> AuthorNames { get; set; }

		public BookDto()
		{
		}

		public BookDto(Book book)
		{
			BookBarcode = book.Barcode;
			BookISBN = book.ISBN;
			BookTitle = book.Title;
			BookEdition = book.Edition;
			BookPrice = book.Price;
			BookDescription = book.Description;
			BookPublishedYear = book.PublishedYear;
			BookStatus = book.Status;
			BookType = book.Type.Name;
			IsBookBorrowable = book.Type.IsBorrowable;
			PublisherName = book.Publisher.Name;
			LibraryName = book.Library.Name;
			AuthorNames = book.Authors.Select(a => a.Author.Name);
		}
	}

	public class GetBookByBarcode : IQuery
	{
		public int Barcode { get; set; }
	}

	public class GetBookByBarcodeQueryHandler : AsyncQueryHandlerBase<GetBookByBarcode, Option<Book>>
	{
		public GetBookByBarcodeQueryHandler(LibrarymanContext context) : base(context)
		{
		}

		public override async Task<Option<Book>> HandleAsync(GetBookByBarcode query)
		{
			Book book = await _context
				.Books
				.AsNoTracking()
				.Include(b => b.Type)
				.Include(b => b.Publisher)
				.Include(b => b.Publisher)
				.Include(b => b.Authors.Select(a => a.Author))
				.Include(b => b.Library)
				.SingleOrDefaultAsync(b => b.Barcode == query.Barcode)
				.ConfigureAwait(false);
			return book.SomeNotNull();
		}
	}

	public class IssueBookToUser : ICommand
	{
		public int UserId { get; set; }
		public int BookBarcode { get; set; }
		public int StaffId { get; set; }
	}

	public class IssueBookToUserCommandHandler : AsyncCommandHandlerBase<IssueBookToUser, Result>
	{
		public IssueBookToUserCommandHandler(LibrarymanContext context) : base(context)
		{
		}

		public override async Task<Result> HandleAsync(IssueBookToUser command)
		{
			User user = await _context
				.Users
				.Include(u => u.Type)
				.SingleOrDefaultAsync(u => u.Id == command.UserId)
				.ConfigureAwait(false);

			if (user == null) return Result.Fail($"User with id '{command.UserId} not found.");


			DateTime now = DateTime.Now;

			var record = new Record()
			{
				BookBarcode = command.BookBarcode,
				StaffId = command.StaffId,
				Timestamp = DateTime.Now,
				Type = RecordType.Issue,
				UserId = command.UserId
			};

			DateTime dueDate = now.Date + user.Type.Duration;

			var borrowedBook = new BorrowedBook()
			{
				BookBarcode = command.BookBarcode,
				DueDate = dueDate,
				UserId = command.UserId,
				Record = record
			};

			_context.BorrowedBooks.Add(borrowedBook);
			await _context.SaveChangesAsync().ConfigureAwait(false);
			return Result.Ok();
		}
	}

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

			await _commandDispatcher.DispatchAsync<IssueBookToUser, Result>(
					new IssueBookToUser() {BookBarcode = bookBarcode, StaffId = staffId, UserId = userId})
				.ConfigureAwait(false);

			Searcher.ClearSearchString();
			_navigation.GoBack<IssueViewModel>(vm => { vm.LoadDetailsCommand.Execute(null); });
		}

		private bool CanAddBook()
		{
			return Searcher.SearchResult.IsBookBorrowable && Searcher.SearchResult.BookStatus == BookStatus.Available;
		}

		private async Task<Option<BookDto>> OnSearch(string search)
		{
			Option<Book> book = await _queryDispatcher.DispatchAsync<GetBookByBarcode, Option<Book>>(
					new GetBookByBarcode() {Barcode = int.Parse(Searcher.SearchString)})
				.ConfigureAwait(false);

			return book.Map(b => new BookDto(b));
		}
	}
}