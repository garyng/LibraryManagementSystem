using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Libraryman.Entity
{
	public class Book
	{
		public int Barcode { get; set; }
		[MaxLength(20)]
		public string ISBN { get; set; }
		[MaxLength(255)]
		public string Title { get; set; }
		[MaxLength(255)]
		public string Edition { get; set; }
		public decimal Price { get; set; }
		[MaxLength(2000)]
		public string Description { get; set; }
		[MaxLength(5)]
		public string PublishedYear { get; set; }
		
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