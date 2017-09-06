using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Libraryman.Entity
{
	public class User
	{
		public int Id { get; set; }
		[MaxLength(255)]
		public string Name { get; set; }
		public Gender Gender { get; set; }
		[MaxLength(15)]
		public string PhoneNumber { get; set; }
		[MaxLength(255)]
		public string Email { get; set; }
		
		public MembershipType Type { get; set; }

		public ICollection<Record> Records { get; set; }
		public ICollection<BorrowedBook> BorrowedBooks { get; set; }

	}
}