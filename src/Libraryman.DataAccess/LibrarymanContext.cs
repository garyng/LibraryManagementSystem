using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Libraryman.Entity;

namespace Libraryman.DataAccess
{
	public class LibrarymanContext : DbContext
	{
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Book>()
				.HasKey(b => b.Barcode);

			modelBuilder.Entity<User>()
				.Property(u => u.Email)
				.IsOptional();
			modelBuilder.Entity<User>()
				.HasRequired(u => u.Type);

			modelBuilder.Entity<AuthorBook>()
				.HasKey(ab => new {ab.AuthorId, ab.BookId});
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
	}
}