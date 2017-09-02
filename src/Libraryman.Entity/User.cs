using System.Collections.Generic;

namespace Libraryman.Entity
{
	public class User
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public Gender Gender { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }
		
		public MembershipType Type { get; set; }

		public ICollection<Record> Records { get; set; }
		public ICollection<BorrowedBook> BorrowedBooks { get; set; }

	}
}