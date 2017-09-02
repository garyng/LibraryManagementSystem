using System;

namespace Libraryman.Entity
{
	public class Record
	{
		public int Id { get; set; }
		public RecordType Type { get; set; }
		public DateTime Timestamp { get; set; }

		public User User { get; set; }
		public int UserId { get; set; }

		public Staff Staff { get; set; }
		public int StaffId { get; set; }

		public Book Book { get; set; }
		public int BookBarcode { get; set; }

	}
}