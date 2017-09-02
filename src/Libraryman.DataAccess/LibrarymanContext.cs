﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Libraryman.Entity;
using MoreLinq;

namespace Libraryman.DataAccess
{
	public class LibrarymanContext : DbContext
	{
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			System.Data.Entity.Database.SetInitializer(new LibrarymanInitializer());
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

		private class LibrarymanInitializer : CreateDatabaseIfNotExists<LibrarymanContext>
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
					.Generate(50);

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
					.Generate(10);

				var books = new Faker<Book>()
					.RuleFor(b => b.Title, f => f.Lorem.Sentence())
					.RuleFor(b => b.Edition, f => f.PickRandom(new List<string>() {"1st Edition", "2nd Edition", "2rd Edition"}))
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
					.Generate(600)
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

				var records = new Faker<Record>()
					.RuleFor(r => r.Type, f => f.PickRandom<RecordType>())
					.RuleFor(r => r.Timestamp, f => f.Date.Recent())
					.RuleFor(r => r.User, f => f.PickRandom(users))
					.RuleFor(r => r.Staff, f => f.PickRandom(staffs))
					.RuleFor(r => r.Book, f => f.PickRandom(books))
					.Generate(500);


				context.Libraries.AddRange(libraries);
				context.Staffs.AddRange(staffs);
				context.BookTypes.AddRange(bookTypes);
				context.Publishers.AddRange(publishers);
				context.Books.AddRange(books);
				context.Authors.AddRange(authors);
				context.AuthorBooks.AddRange(authorBooks);
				context.MembershipTypes.AddRange(membershipTypes);
				context.Users.AddRange(users);
				context.Records.AddRange(records);

				base.Seed(context);
			}
		}
	}
}