using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using Libraryman.Entity;
using Libraryman.Wpf.Command;
using Libraryman.Wpf.Dto;
using Libraryman.Wpf.Navigation;
using Libraryman.Wpf.Query;
using MaterialDesignThemes.Wpf;
using Optional;

namespace Libraryman.Wpf.UserInfo
{
	public class SearchUserViewModel : ViewModelBase
	{
		public RelayCommand IssueBookCommand { get; set; }

		public EntitySearcher<UserDto> Searcher { get; set; }

		public SearchUserViewModel(INavigationService<ViewModelBase> navigation, IAsyncCommandDispatcher commandDispatcher,
			IAsyncQueryDispatcher queryDispatcher, ISnackbarMessageQueue snackbarMessageQueue) : base(navigation,
			commandDispatcher, queryDispatcher, snackbarMessageQueue)
		{
			Searcher = new EntitySearcher<UserDto>(async search => await OnSearch(search).ConfigureAwait(false));
			IssueBookCommand = new RelayCommand(OnGoToIssueBook);
#if DEBUG
			if (IsInDesignModeStatic)
			{
				Searcher.SearchResult = new UserDto()
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

		private async Task<Option<UserDto>> OnSearch(string searchString)
		{
			Option<User> option = await _queryDispatcher.DispatchAsync<GetUserDetailsById, Option<User>>(
					new GetUserDetailsById() {UserId = int.Parse(searchString)})
				.ConfigureAwait(false);
			return option.Map(user => new UserDto(user));
		}

		private void OnGoToIssueBook()
		{
			_navigation.GoTo<UserInfoViewModel>(vm =>
			{
				vm.User = Searcher.SearchResult;
				vm.LoadDetailsCommand?.Execute(null);
			});
			Searcher.ClearSearchString();
		}
	}
}