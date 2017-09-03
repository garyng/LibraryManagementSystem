using System;

namespace Libraryman.Wpf.Navigation
{
	public interface INavigationService<TViewModel> where TViewModel : class, INavigationTarget
	{
		bool CanGoBack();
		void GoBack();
		void GoBack(Action<TViewModel> setup);
		void GoBack<TTarget>(Action<TTarget> setup) where TTarget : class, TViewModel;
		void GoTo<TTarget>() where TTarget : class, TViewModel;
		void GoTo<TTarget>(Action<TTarget> setup) where TTarget : class, TViewModel;
		void Register(TViewModel target);
	}
}