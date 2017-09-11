using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Optional;

namespace Libraryman.Wpf
{
	public class EntitySearcher<TSearchResult> : ObservableObject
	{
		private readonly Func<string, Task<Option<TSearchResult>>> _onAsyncSearchFunc;
		private readonly Func<string, bool> _canSearchFunc;
		private string _searchString;

		public string SearchString
		{
			get => _searchString;
			set
			{
				Set(ref _searchString, value);
				SearchCommand?.RaiseCanExecuteChanged();
				IsFound = Option.None<bool>();
			}
		}

		private Option<bool> _isFound;

		public Option<bool> IsFound
		{
			get => _isFound;
			set => Set(ref _isFound, value);
		}

		private TSearchResult _searchResult;

		public TSearchResult SearchResult
		{
			get => _searchResult;
			set => Set(ref _searchResult, value);
		}

		public RelayCommand SearchCommand { get; set; }

		public EntitySearcher(Func<string, Task<Option<TSearchResult>>> onAsyncSearchFunc,
			Func<string, bool> canSearchFunc = null)
		{
			_onAsyncSearchFunc = onAsyncSearchFunc;
			_canSearchFunc = canSearchFunc ??
			                 (search => search?.Length > 0 && int.TryParse(search, out int _));

			SearchCommand = new RelayCommand(async () => await OnSearch().ConfigureAwait(false), CanSearch);
		}

		private bool CanSearch()
		{
			return _canSearchFunc(_searchString);
		}

		private async Task OnSearch()
		{
			Option<TSearchResult> result = await _onAsyncSearchFunc(_searchString).ConfigureAwait(false);
			IsFound = result.Match(
				some: r =>
				{
					SearchResult = r;
					return Option.Some(true);
				},
				none: () => Option.Some(false));
		}
	}
}