using GalaSoft.MvvmLight.Command;
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

		public RelayCommand GoBackCommand { get; set; }

		public ShellHeaderViewModel(INavigationService<ViewModelBase> navigation, AuthenticationState @as) : base(navigation)
		{
			AuthenticationState = @as;
			GoBackCommand = new RelayCommand(OnGoBack);
#if DEBUG
			if (IsInDesignModeStatic)
			{
				IsHamburgerButtonToggled = false;
			}

#endif
		}
		private void OnGoBack()
		{
			if (_navigation.CanGoBack())
			{
				_navigation.GoBack();
			}
		}
	}
}