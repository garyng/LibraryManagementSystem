using Libraryman.Wpf.Navigation;

namespace Libraryman.Wpf.Dashboard
{
	public class DashboardViewModel : ViewModelBase
	{
		public DashboardViewModel(INavigationService<ViewModelBase> navigation) : base(navigation)
		{
		}
	}
}