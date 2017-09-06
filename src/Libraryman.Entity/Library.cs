using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Libraryman.Entity
{
	public class Library
	{
		public int Id { get; set; }
		[MaxLength(255)]
		public string Name { get; set; }
		[MaxLength(255)]
		public string Location { get; set; }

		public ICollection<Book> Books { get; set; } = new HashSet<Book>();
		public ICollection<Staff> Staffs { get; set; } = new HashSet<Staff>();
	}
}