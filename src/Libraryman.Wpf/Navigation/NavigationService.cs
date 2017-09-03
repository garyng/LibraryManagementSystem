using System;
using System.Collections.Generic;
using System.Linq;

namespace Libraryman.Wpf.Navigation
{
	public class NavigationService<TViewModel> : INavigationService<TViewModel>
		where TViewModel : class, INavigationTarget
	{
		private readonly INavigationHost<TViewModel> _host;
		private HashSet<INavigationTarget> _targets = new HashSet<INavigationTarget>();

		private INavigationTarget _current;
		private Stack<INavigationTarget> _history = new Stack<INavigationTarget>();


		public NavigationService(INavigationHost<TViewModel> host)
		{
			_host = host;
		}

		public void Register(TViewModel target)
		{
			_targets.Add(target);
		}

		public void GoTo<TTarget>() where TTarget : class, TViewModel
		{
			GoTo<TTarget>((_) => { });
		}

		public void GoTo<TTarget>(Action<TTarget> setup) where TTarget : class, TViewModel
		{
			INavigationTarget target = _targets.FirstOrDefault(t => t.GetType() == typeof(TTarget));
			if (target != null)
			{
				_host.SetCurrentViewModel(target as TViewModel);
				setup?.Invoke(target as TTarget);

				if (_current != null)
				{
					_history.Push(_current);
				}
				_current = target;
			}
		}

		public void GoBack()
		{
			GoBack((_) => { });
		}

		public void GoBack(Action<TViewModel> setup)
		{
			INavigationTarget target = _history.Pop();
			setup?.Invoke(target as TViewModel);
			_host.SetCurrentViewModel(target as TViewModel);
			_current = target;
		}

		public void GoBack<TTarget>(Action<TTarget> setup) where TTarget : class, TViewModel
		{
			INavigationTarget target = _history.Peek();
			if (!(target is TTarget)) return;

			target = _history.Pop();
			setup?.Invoke(target as TTarget);
			_host.SetCurrentViewModel(target as TViewModel);
			_current = target;
		}

		public bool CanGoBack()
		{
			return _history.Count > 0;
		}
	}
}