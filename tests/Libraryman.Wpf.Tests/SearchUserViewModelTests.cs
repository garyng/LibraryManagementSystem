using System;
using System.Collections.Generic;
using FakeItEasy;
using Libraryman.Entity;
using Libraryman.Wpf.Command;
using Libraryman.Wpf.Issue;
using Libraryman.Wpf.Navigation;
using Libraryman.Wpf.Query;
using MaterialDesignThemes.Wpf;
using NUnit.Framework;
using Optional;

namespace Libraryman.Wpf.Tests
{
	public class SearchUserViewModelTests
	{
		// - should able to validate search string
		// - should disable command execution when validation failed
		// - should set is found according to return result
		// - should reset isfound to None if -> at start
		// -							     -> user alter search string
		// - should raise can execute change if search string changes

		private INavigationService<ViewModelBase> _navigationService;
		private IAsyncQueryDispatcher _queryDispatcher;
		private IAsyncCommandDispatcher _commandDispatcher;
		private ISnackbarMessageQueue _snackbarMessageQueue;

		[SetUp]
		public void Setup()
		{
			_navigationService = A.Fake<INavigationService<ViewModelBase>>();
			_queryDispatcher = A.Fake<IAsyncQueryDispatcher>();
			_commandDispatcher = A.Fake<IAsyncCommandDispatcher>();
			_snackbarMessageQueue = A.Fake<ISnackbarMessageQueue>();
		}

		[TestCase("123", true)]
		[TestCase("", false)]
		[TestCase("asd", false)]
		public void Should_AbleToValidateSearchString(string searchString, bool expected)
		{
			// Arrange


			// Act
			var searchUserViewModel = new SearchUserViewModel(_navigationService, _commandDispatcher, _queryDispatcher,
				_snackbarMessageQueue);
			searchUserViewModel.Searcher.SearchString = searchString;

			// Assert
			Assert.That(searchUserViewModel.Searcher.SearchCommand.CanExecute(null), Is.EqualTo(expected));
		}

		[Test]
		public void Should_SetIsFoundToTrue()
		{
			// Arrange
			A.CallTo(() => _queryDispatcher.DispatchAsync<GetUserDetailsById, Option<User>>(A<GetUserDetailsById>._))
				.Returns(Option.Some(new User()
				{
					Id = 123,
					Name = "Testing Name",
					Gender = Gender.Female,
					PhoneNumber = "123",
					Email = "a@a.com",
					Type = new MembershipType {Name = "Test Type"},
					Records = new List<Record>(),
					BorrowedBooks = new List<BorrowedBook>()
				}));

			var searchUserViewModel = new SearchUserViewModel(_navigationService, _commandDispatcher, _queryDispatcher,
				_snackbarMessageQueue);
			searchUserViewModel.Searcher.SearchString = "123";
			// Act

			searchUserViewModel.Searcher.SearchCommand.Execute(null);

			// Assert
			Assert.That(searchUserViewModel.Searcher.IsFound.Contains(true), Is.True);
		}

		[Test]
		public void Should_SetIsFoundToFalse()
		{
			// Arrange
			A.CallTo(() => _queryDispatcher.DispatchAsync<GetUserDetailsById, Option<User>>(A<GetUserDetailsById>._))
				.Returns(Option.None<User>());

			var searchUserViewModel = new SearchUserViewModel(_navigationService, _commandDispatcher, _queryDispatcher,
				_snackbarMessageQueue);
			searchUserViewModel.Searcher.SearchString = "123";

			// Act

			searchUserViewModel.Searcher.SearchCommand.Execute(null);

			// Assert
			Assert.That(searchUserViewModel.Searcher.IsFound.Contains(true), Is.False);
		}

		[Test]
		public void Should_SetIsFoundToNone_AtStartup()
		{
			// Arrange
			var searchUserViewModel = new SearchUserViewModel(_navigationService, _commandDispatcher, _queryDispatcher,
				_snackbarMessageQueue);

			// Act


			// Assert
			Assert.That(searchUserViewModel.Searcher.IsFound.HasValue, Is.False);
		}

		[Test]
		public void Should_SetIsFoundToNone_If_SearchStringChanges()
		{
			// Arrange
			var searchUserViewModel = new SearchUserViewModel(_navigationService, _commandDispatcher, _queryDispatcher,
				_snackbarMessageQueue);
			searchUserViewModel.Searcher.SearchString = "123";

			// emulate search
			A.CallTo(() => _queryDispatcher.DispatchAsync<GetUserDetailsById, Option<User>>(A<GetUserDetailsById>._))
				.Returns(Option.None<User>());
			searchUserViewModel.Searcher.SearchCommand.Execute(null);

			// Act
			searchUserViewModel.Searcher.SearchString = "Changed";

			// Assert
			Assert.That(searchUserViewModel.Searcher.IsFound.HasValue, Is.False);
		}


		[Test]
		public void Should_RaiseCanExecuteChange_If_SearchStringChanges()
		{
			// Arrange
			var searchUserViewModel = new SearchUserViewModel(_navigationService, _commandDispatcher, _queryDispatcher,
				_snackbarMessageQueue);
			var eventHandler = A.Fake<EventHandler>();
			searchUserViewModel.Searcher.SearchCommand.CanExecuteChanged += eventHandler;

			// Act
			searchUserViewModel.Searcher.SearchString = "Changed";

			// Assert
			A.CallTo(eventHandler).MustHaveHappened();
		}
	}
}