using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Libraryman.Common.Result;
using Libraryman.DataAccess;
using Libraryman.Entity;

namespace Libraryman.Wpf.Command
{
	public class IssueBookToUserCommandHandler : AsyncCommandHandlerBase<IssueBookToUser, Result>
	{
		public IssueBookToUserCommandHandler(LibrarymanContext context) : base(context)
		{
		}

		public override async Task<Result> HandleAsync(IssueBookToUser command)
		{
			User user = await _context
				.Users
				.Include(u => u.BorrowedBooks)
				.Include(u => u.Type)
				.SingleOrDefaultAsync(u => u.Id == command.UserId)
				.ConfigureAwait(false);

			if (user == null) return Result.Fail($"User with id '{command.UserId} not found.");
			if (user.BorrowedBooks.Count >= user.Type.MaximumBooks) return Result.Fail($"User with id '{user.Id}' has borrowed the maximum allowed books!");

			Book book = await _context
				.Books
				.FindAsync(command.BookBarcode)
				.ConfigureAwait(false);

			if (book == null) return Result.Fail($"Book with barcode '{command.BookBarcode} not found.");

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

			book.Status = BookStatus.NotAvailable;
			_context.BorrowedBooks.Add(borrowedBook);
			await _context.SaveChangesAsync().ConfigureAwait(false);
			return Result.Ok();
		}
	}
}