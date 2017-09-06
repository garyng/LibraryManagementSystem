using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Libraryman.Entity
{
	public class Staff
	{
		public int Id { get; set; }
		[MaxLength(255)]
		public string Name { get; set; }
		public Gender Gender { get; set; }
		[MaxLength(15)]
		public string PhoneNumber { get; set; }
		[MaxLength(256)]
		public string PasswordHash { get; set; }
		public DateTime LastLogin { get; set; }

		public Library Library { get; set; }
		public int LibraryId { get; set; }

		public ICollection<Record> Records { get; set; }
	}
}