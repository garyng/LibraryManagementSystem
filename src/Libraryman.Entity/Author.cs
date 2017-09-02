using System.Collections.Generic;

namespace Libraryman.Entity
{
	public class Author
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public Publisher Publisher { get; set; }
		public int PublisherId { get; set; }

		public ICollection<AuthorBook> Books { get; set; }
	}
}