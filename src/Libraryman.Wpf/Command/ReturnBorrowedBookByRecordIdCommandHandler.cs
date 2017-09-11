using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Libraryman.Common.Result;
using Libraryman.DataAccess;
using Libraryman.Entity;

namespace Libraryman.Wpf.Command
{
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
				.Include(r => r.User.Type)
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
}