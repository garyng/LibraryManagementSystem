using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Bogus;
using Libraryman.Common.Extensions;
using Libraryman.Entity;
using MoreLinq;

namespace Libraryman.DataAccess
{
	public class LibrarymanContext : DbContext
	{
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			// System.Data.Entity.Database.SetInitializer(new LibrarymanInitializer());
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			modelBuilder.Entity<Book>()
				.HasKey(b => b.Barcode);

			modelBuilder.Entity<User>()
				.Property(u => u.Email)
				.IsOptional();

			modelBuilder.Entity<User>()
				.HasRequired(u => u.Type);

			modelBuilder.Entity<AuthorBook>()
				.HasKey(ab => new {ab.AuthorId, ab.BookBarcode});
			modelBuilder.Entity<AuthorBook>()
				.HasRequired(ab => ab.Book)
				.WithMany(b => b.Authors);
			modelBuilder.Entity<AuthorBook>()
				.HasRequired(ab => ab.Author)
				.WithMany(a => a.Books);

			modelBuilder.Entity<BorrowedBook>()
				.HasKey(bb => new {bb.RecordId, bb.UserId, bb.BookBarcode});

			modelBuilder.Entity<ReturnedBook>()
				.HasKey(rb => new {rb.RecordId, rb.UserId, rb.BookBarcode});


			base.OnModelCreating(modelBuilder);
		}

		public DbSet<Library> Libraries { get; set; }
		public DbSet<Staff> Staffs { get; set; }
		public DbSet<Book> Books { get; set; }
		public DbSet<BookType> BookTypes { get; set; }
		public DbSet<Publisher> Publishers { get; set; }
		public DbSet<Author> Authors { get; set; }
		public DbSet<AuthorBook> AuthorBooks { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Record> Records { get; set; }
		public DbSet<MembershipType> MembershipTypes { get; set; }
		public DbSet<BorrowedBook> BorrowedBooks { get; set; }
		public DbSet<ReturnedBook> ReturnedBooks { get; set; }

		private class LibrarymanInitializer : DropCreateDatabaseAlways<LibrarymanContext>
		{
			protected override void Seed(LibrarymanContext context)
			{
				var libraries = new Faker<Library>()
					.RuleFor(l => l.Location, lf => lf.Address.City())
					.RuleFor(l => l.Name, (lf, l) => $"Ohaiko Central Library ({l.Location})")
					.Generate(5);

				var staffs = new Faker<Staff>()
					.RuleFor(s => s.Name, f => f.Name.FullName())
					.RuleFor(s => s.Gender, f => f.PickRandom<Gender>())
					.RuleFor(s => s.PhoneNumber, f => f.Phone.PhoneNumber("+###-#########"))
					.RuleFor(s => s.PasswordHash, f => f.Random.AlphaNumeric(256))
					.RuleFor(s => s.LastLogin, f => f.Date.Past())
					.RuleFor(s => s.Library, f => f.PickRandom(libraries))
					.Generate(10);

				staffs.Add(new Staff()
				{
					Name = "Gary Ng",
					Gender = Gender.Male,
					PhoneNumber = "+012-345677890",
					PasswordHash = "garyng".ToSHA256(),
					LastLogin = DateTime.Now,
					Library = libraries[0]
				});

				var bookTypes = new List<BookType>()
				{
					new BookType()
					{
						Name = "Reference",
						IsBorrowable = false
					},
					new BookType()
					{
						Name = "Normal",
						IsBorrowable = true
					}
				};

				var publishers = new Faker<Publisher>()
					.RuleFor(p => p.Name, f => f.Company.CompanyName())
					.RuleFor(p => p.ContactNumber, f => f.Phone.PhoneNumber("0#-### ####"))
					.RuleFor(p => p.Address, f => f.Address.FullAddress())
					.Generate(50);

				var books = new Faker<Book>()
					.RuleFor(b => b.Title, f => f.Lorem.Sentence())
					.RuleFor(b => b.ISBN, f => f.Random.ReplaceNumbers("###-##-#####-##-#"))
					.RuleFor(b => b.Edition,
						f => f.PickRandom(new List<string>() {"1st Edition", "2nd Edition", "3rd Edition", "4th Edition", "5th Edition"}))
					.RuleFor(b => b.Price, f => f.Random.Decimal(0, 100))
					.RuleFor(b => b.Description, f => f.Lorem.Paragraphs())
					.RuleFor(b => b.PublishedYear, f => f.Date.Past().Year.ToString())
					.RuleFor(b => b.Status, f => f.PickRandom<BookStatus>())
					.RuleFor(b => b.Type, f => f.PickRandom(bookTypes))
					.RuleFor(b => b.Publisher, f => f.PickRandom(publishers))
					.RuleFor(b => b.Library, f => f.PickRandom(libraries))
					.Generate(500);

				var authors = new Faker<Author>()
					.RuleFor(a => a.Name, f => f.Name.FullName())
					.RuleFor(a => a.Publisher, f => f.PickRandom(publishers))
					.Generate(100);

				var authorBooks = new Faker<AuthorBook>()
					.RuleFor(ab => ab.Author, f => f.PickRandom(authors))
					.RuleFor(ab => ab.Book, f => f.PickRandom(books))
					.Generate(500)
					.DistinctBy(ab => new {ab.Book, ab.Author});

				var membershipTypes = new List<MembershipType>()
				{
					new MembershipType()
					{
						Name = "NonMember",
						Duration = TimeSpan.FromDays(14),
						MaximumBooks = 5,
						OverdueFine = 0.20M
					},
					new MembershipType()
					{
						Name = "Member",
						Duration = TimeSpan.FromDays(28),
						MaximumBooks = 10,
						OverdueFine = 0.10M
					}
				};

				var users = new Faker<User>()
					.RuleFor(u => u.Name, f => f.Name.FullName())
					.RuleFor(u => u.Gender, f => f.PickRandom<Gender>())
					.RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber("+###-#########"))
					.RuleFor(u => u.Email, f => f.Internet.Email())
					.RuleFor(u => u.Type, f => f.PickRandom(membershipTypes))
					.Generate(100);

				//var records = new Faker<Record>()
				//	.RuleFor(r => r.Type, f => f.PickRandom<RecordType>())
				//	.RuleFor(r => r.Timestamp,
				//		f => f.Date.Recent(100))
				//	.RuleFor(r => r.User, f => f.PickRandom(users))
				//	.RuleFor(r => r.Staff, f => f.PickRandom(staffs))
				//	.RuleFor(r => r.Book, f => f.PickRandom(books))
				//	.Generate(500);

				var borrowedBooks = new Faker<BorrowedBook>()
					.RuleFor(bb => bb.Record, f => new Faker<Record>()
						.RuleFor(r => r.Type, rf => RecordType.Issue)
						.RuleFor(r => r.Timestamp, rf => DateTime.Now)
						.RuleFor(r => r.User, rf => rf.PickRandom(users))
						.RuleFor(r => r.Staff, rf => rf.PickRandom(staffs))
						.RuleFor(r => r.Book, rf => rf.PickRandom(books))
						.Generate())
					.RuleFor(bb => bb.Book, (f, bb) => bb.Record.Book)
					.RuleFor(bb => bb.User, (f, bb) => bb.Record.User)
					.RuleFor(bb => bb.DueDate, (f, bb) => bb.Record.Timestamp + TimeSpan.FromDays(14))
					.Generate(200);

				var returnedBooks = new Faker<ReturnedBook>()
					.RuleFor(rb => rb.Record, f => new Faker<Record>()
						.RuleFor(r => r.Type, rf => RecordType.Return)
						.RuleFor(r => r.Timestamp, rf => rf.Date.Recent(100))
						.RuleFor(r => r.User, rf => rf.PickRandom(users))
						.RuleFor(r => r.Staff, rf => rf.PickRandom(staffs))
						.RuleFor(r => r.Book, rf => rf.PickRandom(books))
						.Generate())
					.RuleFor(rb => rb.Book, (f, rb) => rb.Record.Book)
					.RuleFor(rb => rb.User, (f, rb) => rb.Record.User)
					.RuleFor(rb => rb.BorrowingRecord, (f, rb) => new Faker<Record>()
						.RuleFor(r => r.Type, rf => RecordType.Issue)
						.RuleFor(r => r.Timestamp,
							rf => rf.Date.Between(rb.Record.Timestamp - TimeSpan.FromDays(30), rb.Record.Timestamp))
						.RuleFor(r => r.User, rf => rf.PickRandom(users))
						.RuleFor(r => r.Staff, rf => rf.PickRandom(staffs))
						.RuleFor(r => r.Book, rf => rf.PickRandom(books))
						.Generate())
					.RuleFor(rb => rb.OverdueFine,
						(f, rb) => (rb.Record.Timestamp - rb.BorrowingRecord.Timestamp).Days * rb.Record.User.Type.OverdueFine)
					.Generate(400);
				//.DistinctBy(rb => rb.BorrowingRecord)
				//.DistinctBy(rb => rb.Record);

				context.Database.ExecuteSqlCommand("ALTER TABLE staff AUTO_INCREMENT=1000");
				context.Database.ExecuteSqlCommand("ALTER TABLE user AUTO_INCREMENT=1000000");

				context.Libraries.AddRange(libraries);
				context.Staffs.AddRange(staffs);
				context.BookTypes.AddRange(bookTypes);
				context.Publishers.AddRange(publishers);
				context.Books.AddRange(books);
				context.Authors.AddRange(authors);
				context.AuthorBooks.AddRange(authorBooks);
				context.MembershipTypes.AddRange(membershipTypes);
				context.Users.AddRange(users);
				// context.Records.AddRange(records);
				context.BorrowedBooks.AddRange(borrowedBooks);
				context.ReturnedBooks.AddRange(returnedBooks);

				base.Seed(context);
			}
		}
	}
}