using System;
using System.Collections.Generic;

namespace Libraryman.Entity
{
	public class MembershipType
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public TimeSpan Duration { get; set; }
		public int MaximumBooks { get; set; }
		public decimal OverdueFine { get; set; }

		public ICollection<User> Users { get; set; }
	}
}