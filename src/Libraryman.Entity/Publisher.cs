using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Libraryman.Entity
{
	public class Publisher
	{
		public int Id { get; set; }
		[MaxLength(255)]
		public string Name { get; set; }
		[MaxLength(15)]
		public string ContactNumber { get; set; }
		[MaxLength(500)]
		public string Address { get; set; }

		public ICollection<Author> Authors { get; set; }
		public ICollection<Book> Books { get; set; }

	}
}