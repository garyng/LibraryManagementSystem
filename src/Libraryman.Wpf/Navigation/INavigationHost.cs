namespace Libraryman.Wpf.Navigation
{
	public interface INavigationHost<TViewModel>
	{
		void SetCurrentViewModel(TViewModel target);
	}
}