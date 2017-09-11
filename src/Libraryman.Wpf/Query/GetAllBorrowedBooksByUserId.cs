namespace Libraryman.Wpf.Query
{
	public class GetAllBorrowedBooksByUserId : IQuery
	{
		public int UserId { get; set; }
	}
}