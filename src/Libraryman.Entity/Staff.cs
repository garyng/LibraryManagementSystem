using System;
using System.Collections.Generic;

namespace Libraryman.Entity
{
	public class Staff
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public Gender Gender { get; set; }
		public string PhoneNumber { get; set; }

		public string PasswordHash { get; set; }
		public DateTime LastLogin { get; set; }

		public Library Library { get; set; }
		public int LibraryId { get; set; }

		public ICollection<Record> Records { get; set; }
	}
}