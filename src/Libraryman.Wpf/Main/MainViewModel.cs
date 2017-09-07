using Libraryman.Wpf.Navigation;
using Libraryman.Wpf.Service;

namespace Libraryman.Wpf.Main
{
	public class MainViewModel : ViewModelBase
	{
		private readonly AuthenticationState _authenticationState;

		public MainViewModel(INavigationService<ViewModelBase> navigation, AuthenticationState authenticationState) : base(navigation)
		{
			_authenticationState = authenticationState;
		}

	}
}