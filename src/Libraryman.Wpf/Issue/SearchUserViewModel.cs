using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using Libraryman.Entity;
using Libraryman.Wpf.Dto;
using Libraryman.Wpf.Navigation;
using Libraryman.Wpf.Query;
using Optional;

namespace Libraryman.Wpf.Issue
{
	public class SearchUserViewModel : ViewModelBase
	{
		private readonly IAsyncQueryDispatcher _queryDispatcher;
		private string _searchString;

		public string SearchString
		{
			get => _searchString;
			set
			{
				Set(ref _searchString, value);
				SearchCommand?.RaiseCanExecuteChanged();
				if (SearchString.Length == 0)
				{
					IsFound = Option.None<bool>();
				}
				if (IsFound.Contains(false) && SearchString.Length > 0)
				{
					IsFound = Option.None<bool>();
				}
			}
		}

		private Option<bool> _isFound;

		public Option<bool> IsFound
		{
			get => _isFound;
			set => Set(ref _isFound, value);
		}

		private UserDto _searchResult;

		public UserDto SearchResult
		{
			get => _searchResult;
			set => Set(ref _searchResult, value);
		}

		public RelayCommand SearchCommand { get; set; }
		public RelayCommand IssueBookCommand { get; set; }

		public SearchUserViewModel(INavigationService<ViewModelBase> navigation,
			IAsyncQueryDispatcher queryDispatcher)
			: base(navigation)
		{
			_queryDispatcher = queryDispatcher;
			SearchCommand = new RelayCommand(async () => await OnSearch().ConfigureAwait(false), CanSearch);
			IssueBookCommand = new RelayCommand(OnGoToIssueBook);
#if DEBUG
			if (IsInDesignModeStatic)
			{
				SearchResult = new UserDto()
				{
					UserId = 100001,
					Email = "email@email.com",
					InHandBooksCount = 1000,
					MembershipType = "Non Member",
					PhoneNumber = "+60 123 123 123",
					TotalRecordCount = 21234,
					UserGender = "Female",
					UserName = "Testing"
				};
			}
#endif
		}

		private bool CanSearch()
		{
			return _searchString?.Length > 0 && int.TryParse(_searchString, out int _);
		}

		private async Task OnSearch()
		{
			Option<User> result = await _queryDispatcher.DispatchAsync<GetUserDetailsById, Option<User>>(
					new GetUserDetailsById() {UserId = int.Parse(_searchString)})
				.ConfigureAwait(false);

			IsFound = result.Match(
				some: user =>
				{
					SearchResult = new UserDto(user);
					return Option.Some(true);
				},
				none: () => Option.Some(false));
		}

		private void OnGoToIssueBook()
		{
			_navigation.GoTo<IssueViewModel>(vm =>
			{
				vm.User = SearchResult;
				vm.LoadDetailsCommand?.Execute(null);
			});
		}
	}
}