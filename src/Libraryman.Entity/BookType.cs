using System.Collections.Generic;

namespace Libraryman.Entity
{
	public class BookType
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public bool IsBorrowable { get; set; }

		public ICollection<Book> Books { get; set; }
	}
}