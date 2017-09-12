using Libraryman.Wpf.Command;
using Libraryman.Wpf.Navigation;
using Libraryman.Wpf.Query;
using MaterialDesignThemes.Wpf;

namespace Libraryman.Wpf
{
	public abstract class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase, INavigationTarget
	{
		public virtual bool GoBackOnly { get; } = false;

		protected readonly INavigationService<ViewModelBase> _navigation;
		protected readonly IAsyncCommandDispatcher _commandDispatcher;
		protected readonly IAsyncQueryDispatcher _queryDispatcher;
		protected readonly ISnackbarMessageQueue _snackbarMessageQueue;

		protected ViewModelBase(INavigationService<ViewModelBase> navigation,
			IAsyncCommandDispatcher commandDispatcher, IAsyncQueryDispatcher queryDispatcher,
			ISnackbarMessageQueue snackbarMessageQueue)
		{
			_navigation = navigation;
			_commandDispatcher = commandDispatcher;
			_queryDispatcher = queryDispatcher;
			_snackbarMessageQueue = snackbarMessageQueue;
		}
	}
}