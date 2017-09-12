using Libraryman.Wpf.Navigation;
using Libraryman.Wpf.Service;
using MaterialDesignThemes.Wpf;

namespace Libraryman.Wpf.Main
{
	public class MainViewModel : GalaSoft.MvvmLight.ViewModelBase, INavigationHost<ViewModelBase>
	{
		public ISnackbarMessageQueue SnackbarMessageQueue { get; }

		public MainViewModel(ISnackbarMessageQueue snackbarMessageQueue)
		{
			SnackbarMessageQueue = snackbarMessageQueue;
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