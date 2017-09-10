using Libraryman.Wpf.Navigation;

namespace Libraryman.Wpf
{
	public abstract class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase, INavigationTarget
	{
		public virtual bool GoBackOnly { get; } = false;

		protected readonly INavigationService<ViewModelBase> _navigation;

		protected ViewModelBase(INavigationService<ViewModelBase> navigation)
		{
			_navigation = navigation;
		}
	}
}