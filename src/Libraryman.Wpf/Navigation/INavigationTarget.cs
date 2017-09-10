namespace Libraryman.Wpf.Navigation
{
	public interface INavigationTarget
	{
		/// <summary>
		/// Show a back button without the menu toggle button
		/// </summary>
		bool GoBackOnly { get; }
	}
}