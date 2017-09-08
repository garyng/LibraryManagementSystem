namespace Libraryman.Wpf.Query
{
	public class GetBorrowedBookDetailsByBarcode : IQuery
	{
		public int Barcode { get; set; }
	}
}