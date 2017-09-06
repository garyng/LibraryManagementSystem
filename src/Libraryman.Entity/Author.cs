using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Libraryman.Entity
{
	public class Author
	{
		public int Id { get; set; }
		[MaxLength(255)]
		public string Name { get; set; }

		public Publisher Publisher { get; set; }
		public int PublisherId { get; set; }

		public ICollection<AuthorBook> Books { get; set; }
	}
}