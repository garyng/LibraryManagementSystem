using System;
using System.Collections.Generic;

namespace Libraryman.Entity
{
	public class Book
	{
		public int Barcode { get; set; }
		public string Title { get; set; }
		public string Edition { get; set; }
		public decimal Price { get; set; }
		public string Description { get; set; }
		public DateTime PublishedYear { get; set; }
		
		public BookStatus Status { get; set; }

		public BookType Type { get; set; }
		public int TypeId { get; set; }

		public Publisher Publisher { get; set; }
		public int PublisherId { get; set; }

		public Library Library { get; set; }
		public int LibraryId { get; set; }

		public ICollection<AuthorBook> Authors { get; set; }
		public ICollection<Record> Records { get; set; }
		
	}
}