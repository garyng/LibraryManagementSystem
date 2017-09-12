using System.Collections.Generic;
using System.Linq;
using Libraryman.Entity;

namespace Libraryman.Wpf.Dto
{
	public class BookDto
	{
		public int BookBarcode { get; set; }
		public string BookISBN { get; set; }
		public string BookTitle { get; set; }
		public string BookEdition { get; set; }
		public decimal BookPrice { get; set; }
		public string BookDescription { get; set; }
		public string BookPublishedYear { get; set; }
		public BookStatus BookStatus { get; set; }
		public string BookType { get; set; }
		public bool IsBookBorrowable { get; set; }
		public string PublisherName { get; set; }
		public string LibraryName { get; set; }
		public IEnumerable<string> AuthorNames { get; set; }

		public BookDto()
		{
		}

		public BookDto(Book book)
		{
			BookBarcode = book.Barcode;
			BookISBN = book.ISBN;
			BookTitle = book.Title;
			BookEdition = book.Edition;
			BookPrice = book.Price;
			BookDescription = book.Description;
			BookPublishedYear = book.PublishedYear;
			BookStatus = book.Status;
			BookType = book.Type.Name;
			IsBookBorrowable = book.Type.IsBorrowable;
			PublisherName = book.Publisher.Name;
			LibraryName = book.Library.Name;
			AuthorNames = book.Authors.Select(a => a.Author.Name);
		}
	}
}