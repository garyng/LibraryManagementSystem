using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using Libraryman.Wpf.Navigation;

namespace Libraryman.Wpf
{
	public class ShellViewModel : GalaSoft.MvvmLight.ViewModelBase, INavigationHost<ViewModelBase>
	{
		public ShellViewModel()
		{
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