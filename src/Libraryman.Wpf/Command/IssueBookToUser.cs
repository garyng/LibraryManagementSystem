namespace Libraryman.Wpf.Command
{
	public class IssueBookToUser : ICommand
	{
		public int UserId { get; set; }
		public int BookBarcode { get; set; }
		public int StaffId { get; set; }
	}
}