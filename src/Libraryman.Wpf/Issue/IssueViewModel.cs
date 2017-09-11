using System;
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
using MoreLinq;

namespace Libraryman.Wpf.Issue
{
	public class RecordDto
	{
		public int RecordId { get; set; }
		public RecordType RecordType { get; set; }
		public DateTime Timestamp { get; set; }
		public int StaffId { get; set; }
		public string StaffName { get; set; }
		public int BookBarcode { get; set; }
		public string BookISBN { get; set; }
		public string BookTitle { get; set; }
		public string BookEdition { get; set; }
		public string BookPrice { get; set; }
		public string BookDescription { get; set; }
		public string BookPublishedYear { get; set; }
		public string BookStatus { get; set; }
		public string BookType { get; set; }
		public string PublisherName { get; set; }
		public IEnumerable<string> AuthorNames { get; set; }

		public RecordDto()
		{
		}

		public RecordDto(Record record)
		{
			RecordId = record.Id;
			RecordType = record.Type;
			Timestamp = record.Timestamp;
			StaffId = record.StaffId;
			StaffName = record.Staff.Name;
			BookBarcode = record.Book.Barcode;
			BookISBN = record.Book.ISBN;
			BookTitle = record.Book.Title;
			BookEdition = record.Book.Edition;
			BookPrice = record.Book.Price.ToString("0.00");
			BookDescription = record.Book.Description;
			BookPublishedYear = record.Book.PublishedYear;
			BookStatus = record.Book.Status.ToString();
			BookType = record.Book.Type.Name;
			PublisherName = record.Book.Publisher.Name;
			AuthorNames = record.Book.Authors.Select(a => a.Author.Name);
		}
	}

	public class GetAllRecordByUserId : IQuery
	{
		public int UserId { get; set; }
	}

	public class GetAllRecordByUserIdQueryHandler : AsyncQueryHandlerBase<GetAllRecordByUserId, IEnumerable<Record>>
	{
		public GetAllRecordByUserIdQueryHandler(LibrarymanContext context) : base(context)
		{
		}

		public override async Task<IEnumerable<Record>> HandleAsync(GetAllRecordByUserId query)
		{
			List<Record> records = await _context
				.Records
				.Include(r => r.Staff)
				.Include(r => r.Book)
				.Include(r => r.Book.Type)
				.Include(r => r.Book.Publisher)
				.Include(r => r.Book.Authors.Select(a => a.Author))
				.Where(r => r.UserId == query.UserId)
				.ToListAsync()
				.ConfigureAwait(false);
			return records?.Count > 0 ? records : new List<Record>();
		}
	}

	public class GetAllBorrowedBooksByUserId : IQuery
	{
		public int UserId { get; set; }
	}

	public class GetAllBorrowedBooksByUserIdQueryHandler : AsyncQueryHandlerBase<GetAllBorrowedBooksByUserId,
		IEnumerable<BorrowedBook>>
	{
		public GetAllBorrowedBooksByUserIdQueryHandler(LibrarymanContext context) : base(context)
		{
		}

		public override async Task<IEnumerable<BorrowedBook>> HandleAsync(GetAllBorrowedBooksByUserId query)
		{
			List<BorrowedBook> borrowedBooks = await _context
				.BorrowedBooks
				.Include(bb => bb.Record)
				.Include(bb => bb.Book)
				.Include(bb => bb.User)
				.Where(bb => bb.UserId == query.UserId)
				.ToListAsync()
				.ConfigureAwait(false);

			return borrowedBooks?.Count > 0 ? borrowedBooks : new List<BorrowedBook>();
		}
	}

	public class IssueViewModel : ViewModelBase
	{
		private readonly IAsyncQueryDispatcher _queryDispatcher;
		private readonly IAsyncCommandDispatcher _commandDispatcher;
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

		public IssueViewModel(INavigationService<ViewModelBase> navigation,
			IAsyncQueryDispatcher queryDispatcher, IAsyncCommandDispatcher commandDispatcher)
			: base(navigation)
		{
			_queryDispatcher = queryDispatcher;
			_commandDispatcher = commandDispatcher;
			LoadDetailsCommand = new RelayCommand(async () => await OnLoadDetails().ConfigureAwait(false));
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

			IOrderedEnumerable<BorrowedBookDto> borrowedBooks = (await _queryDispatcher.DispatchAsync<GetAllBorrowedBooksByUserId, IEnumerable<BorrowedBook>>(
						new GetAllBorrowedBooksByUserId() {UserId = _user.UserId})
					.ConfigureAwait(false))
				.Select(bb => new BorrowedBookDto(bb))
				.OrderBy(bb => bb.DueDate);
			BorrowedBooks = borrowedBooks.ToObservableCollection();
		}
	}
}