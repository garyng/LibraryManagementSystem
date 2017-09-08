namespace Libraryman.Wpf.Command
{

	public class ReturnBorrowedBookByRecordId : ICommand
	{
		public int RecordId { get; set; }

		public int StaffId { get; set; }
	}
}