using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using Libraryman.Wpf.Dashboard;
using Libraryman.Wpf.Issue;
using Libraryman.Wpf.Navigation;
using Libraryman.Wpf.Return;

namespace Libraryman.Wpf.Shell
{
	public class MainMenuEntry
	{
		public string Title { get; }
		public RelayCommand GoCommand { get; }

		public MainMenuEntry(string title, RelayCommand goCommand)
		{
			Title = title;
			GoCommand = goCommand;
		}
	}

	public class ShellMainMenuViewModel : ViewModelBase
	{
		public ObservableCollection<MainMenuEntry> MainMenuEntries { get; set; }

		public ShellMainMenuViewModel(INavigationService<ViewModelBase> navigation) : base(navigation)
		{
			MainMenuEntries = new ObservableCollection<MainMenuEntry>();
			MainMenuEntries.Add(new MainMenuEntry("Dashboard", new RelayCommand(Go<DashboardViewModel>)));
			MainMenuEntries.Add(new MainMenuEntry("Issue", new RelayCommand(Go<SearchUserViewModel>)));
			MainMenuEntries.Add(new MainMenuEntry("Return", new RelayCommand(Go<ReturnViewModel>)));
#if DEBUG
			if (IsInDesignModeStatic)
			{
				MainMenuEntries.Add(new MainMenuEntry("Dashboard", new RelayCommand(() => { })));
				MainMenuEntries.Add(new MainMenuEntry("Issue", new RelayCommand(() => { })));
				MainMenuEntries.Add(new MainMenuEntry("Return", new RelayCommand(() => { })));
				MainMenuEntries.Add(new MainMenuEntry("Student", new RelayCommand(() => { })));
			}
#endif
		}

		private void Go<TViewModel>() where TViewModel : ViewModelBase
		{
			_navigation.GoTo<TViewModel>();
		}
	}
}