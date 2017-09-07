using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using Libraryman.Wpf.Navigation;
using Libraryman.Wpf.Service;

namespace Libraryman.Wpf
{
	public class ShellViewModel : GalaSoft.MvvmLight.ViewModelBase, INavigationHost<ViewModelBase>
	{
		public AuthenticationState AuthenticationState { get; }

		public ShellViewModel(AuthenticationState @as)
		{
			AuthenticationState = @as;
		}


		private ViewModelBase _currentViewModel;

		public ViewModelBase CurrentViewModel
		{
			get => _currentViewModel;
			set => Set(ref _currentViewModel, value);
		}

		public void SetCurrentViewModel(ViewModelBase target)
		{
			CurrentViewModel = target;
		}
	}
}