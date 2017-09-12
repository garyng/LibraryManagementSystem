using GalaSoft.MvvmLight.Command;
using Libraryman.Wpf.Command;
using Libraryman.Wpf.Navigation;
using Libraryman.Wpf.Query;
using Libraryman.Wpf.Service;
using MaterialDesignThemes.Wpf;

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

		public ShellHeaderViewModel(INavigationService<ViewModelBase> navigation, IAsyncCommandDispatcher commandDispatcher,
			IAsyncQueryDispatcher queryDispatcher, ISnackbarMessageQueue snackbarMessageQueue, AuthenticationState @as) : base(
			navigation, commandDispatcher, queryDispatcher, snackbarMessageQueue)
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