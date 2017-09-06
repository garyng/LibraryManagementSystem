using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Libraryman.Entity
{
	public class BookType
	{
		public int Id { get; set; }
		[MaxLength(255)]
		public string Name { get; set; }

		public bool IsBorrowable { get; set; }

		public ICollection<Book> Books { get; set; }
	}
}