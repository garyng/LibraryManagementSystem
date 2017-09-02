namespace Libraryman.Entity
{
	public class AuthorBook
	{
		public Book Book { get; set; }
		public int BookBarcode { get; set; }

		public Author Author { get; set; }
		public int AuthorId { get; set; }
	}
}