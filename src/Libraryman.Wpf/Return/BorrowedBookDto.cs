using System;
using Libraryman.Entity;

namespace Libraryman.Wpf.Return
{
	public class BorrowedBookDto
	{
		public int BookBarcode { get; set; }
		public string BookISBN { get; set; }
		public string BookTitle { get; set; }
		public string BookEdition { get; set; }
		public string PublishedYear { get; set; }

		public int UserId { get; set; }
		public string UserName { get; set; }
		public string UserType { get; set; }

		public int RecordId { get; set; }
		public DateTime BorrowedDate { get; set; }
		public int StaffId { get; set; }
		public DateTime DueDate { get; set; }

		public bool IsOverdue { get; set; }
		public decimal OverdueFine { get; set; }

		public BorrowedBookDto()
		{
		}

		public BorrowedBookDto(BorrowedBook borrowedBookDetails)
		{
			BookBarcode = borrowedBookDetails.BookBarcode;
			BookISBN = borrowedBookDetails.Book.ISBN;
			BookTitle = borrowedBookDetails.Book.Title;
			BookEdition = borrowedBookDetails.Book.Edition;
			PublishedYear = borrowedBookDetails.Book.PublishedYear;
			UserId = borrowedBookDetails.UserId;
			UserName = borrowedBookDetails.User.Name;
			UserType = borrowedBookDetails.User.Type.Name;
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
}