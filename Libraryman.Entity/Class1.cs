using System;
using System.Collections.Generic;

namespace Libraryman.Entity
{
	public class Library
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Location { get; set; }

		public ICollection<Book> Books { get; set; } = new HashSet<Book>();
		public ICollection<Staff> Staffs { get; set; } = new HashSet<Staff>();


		public Staff Director { get; set; }
		public int DirectorId { get; set; }
	}

	public enum Gender
	{
		Male, 
		Female
	}

	public class Staff
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public Gender Gender { get; set; }
		public string PhoneNumber { get; set; }
		public string Address { get; set; }
		public decimal Salary { get; set; }

		public Job Job { get; set; }
		public int JobId { get; set; }

		public Library Library { get; set; }
		public int LibraryId { get; set; }

		public ICollection<Record> Records { get; set; }
	}

	public class Job
	{
		public int Id { get; set; }
		public string Name { get; set; }


		public ICollection<Staff> Staffs { get; set; }
	}

	public enum BookStatus
	{
		Available,
		NotAvailable
	}

	public class BookType
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public bool IsBorrowable { get; set; }

		public ICollection<Book> Books { get; set; }
	}

	public class Book
	{
		public int Barcode { get; set; }
		public string Title { get; set; }
		public string Edition { get; set; }
		public decimal Price { get; set; }
		public DateTime PublishedAt { get; set; }
		
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

	public class Publisher
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string ContactNumber { get; set; }
		public string Address { get; set; }

		public ICollection<Author> Authors { get; set; }
		public ICollection<Book> Books { get; set; }

	}

	public class AuthorBook
	{
		public Book Book { get; set; }
		public int BookId { get; set; }

		public Author Author { get; set; }
		public int AuthorId { get; set; }
	}

	public class Author
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public Publisher Publisher { get; set; }
		public int PublisherId { get; set; }

		public ICollection<AuthorBook> Books { get; set; }
	}

	public enum RecordType
	{
		Borrowing,
		Returning
	}

	public class Record
	{
		public int Id { get; set; }
		public RecordType Type { get; set; }
		public DateTime DateTime { get; set; }

		public User User { get; set; }
		public int UserId { get; set; }

		public Staff Staff { get; set; }
		public int StaffId { get; set; }

		public Book Book { get; set; }
		public int BookBarcode { get; set; }

	}

	public class User
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public Gender Gender { get; set; }
		public string PhoneNumber { get; set; }
		public string Addres { get; set; }
		public string Email { get; set; }
		
		public MembershipType Type { get; set; }

		public ICollection<Record> Records { get; set; }

	}

	public class MembershipType
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime Duration { get; set; }
		public decimal OverdueFine { get; set; }

		public ICollection<User> Users { get; set; }
	}
}