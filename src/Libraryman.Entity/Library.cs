using System.Collections.Generic;

namespace Libraryman.Entity
{
	public class Library
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Location { get; set; }

		public ICollection<Book> Books { get; set; } = new HashSet<Book>();
		public ICollection<Staff> Staffs { get; set; } = new HashSet<Staff>();
	}
}