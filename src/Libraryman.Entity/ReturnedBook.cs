using System;

namespace Libraryman.Entity
{
	public class ReturnedBook
	{
		public Record Record { get; set; }
		public int RecordId { get; set; }

		public Book Book { get; set; }
		public int BookBarcode { get; set; }

		public User User { get; set; }
		public int UserId { get; set; }

		public Record BorrowingRecord { get; set; }
		public int BorrowingRecordId { get; set; }

		public decimal OverdueFine { get; set; }
	}
}