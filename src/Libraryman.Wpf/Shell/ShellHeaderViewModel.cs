using Libraryman.Wpf.Navigation;
using Libraryman.Wpf.Service;

namespace Libraryman.Wpf.Shell
{
	public class ShellHeaderViewModel : ViewModelBase
	{
		private bool _isHamburgerButtonChecked;

		public bool IsHamburgerButtonToggled
		{
			get => _isHamburgerButtonChecked;
			set => Set(ref _isHamburgerButtonChecked, value);
		}

		public AuthenticationState AuthenticationState { get; }

		public ShellHeaderViewModel(INavigationService<ViewModelBase> navigation, AuthenticationState @as) : base(navigation)
		{
			AuthenticationState = @as;
#if DEBUG
			if (IsInDesignModeStatic)
			{
				IsHamburgerButtonToggled = false;
			}

#endif
		}
	}
}