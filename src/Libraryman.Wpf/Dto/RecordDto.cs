using System;
using System.Collections.Generic;
using System.Linq;
using Libraryman.Entity;

namespace Libraryman.Wpf.Dto
{
	public class RecordDto
	{
		public int RecordId { get; set; }
		public RecordType RecordType { get; set; }
		public DateTime Timestamp { get; set; }
		public int StaffId { get; set; }
		public string StaffName { get; set; }
		public int BookBarcode { get; set; }
		public string BookISBN { get; set; }
		public string BookTitle { get; set; }
		public string BookEdition { get; set; }
		public string BookPrice { get; set; }
		public string BookDescription { get; set; }
		public string BookPublishedYear { get; set; }
		public string BookStatus { get; set; }
		public string BookType { get; set; }
		public string PublisherName { get; set; }
		public IEnumerable<string> AuthorNames { get; set; }

		public RecordDto()
		{
		}

		public RecordDto(Record record)
		{
			RecordId = record.Id;
			RecordType = record.Type;
			Timestamp = record.Timestamp;
			StaffId = record.StaffId;
			StaffName = record.Staff.Name;
			BookBarcode = record.Book.Barcode;
			BookISBN = record.Book.ISBN;
			BookTitle = record.Book.Title;
			BookEdition = record.Book.Edition;
			BookPrice = record.Book.Price.ToString("0.00");
			BookDescription = record.Book.Description;
			BookPublishedYear = record.Book.PublishedYear;
			BookStatus = record.Book.Status.ToString();
			BookType = record.Book.Type.Name;
			PublisherName = record.Book.Publisher.Name;
			AuthorNames = record.Book.Authors.Select(a => a.Author.Name);
		}
	}
}