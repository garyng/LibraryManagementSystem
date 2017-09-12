using System;
using System.Threading.Tasks;
using FakeItEasy;
using NUnit.Framework;
using Optional;

namespace Libraryman.Wpf.Tests
{
	public class EntitySearcherTests
	{
		// - should able to validate search string
		// - should disable command if validation failed
		// - should set is found according to return result
		// - should set search result
		// - should result IsFound to none at startup
		// - should result IsFound to none if search string changes
		// - should raise can execute change if search string changes
		// will call callback when search result changed

		private Func<string, Task<Option<FakeEntity>>> _onSearch;

		public class FakeEntity
		{
			public int Id { get; set; }
		}

		[SetUp]
		public void Setup()
		{
			_onSearch = A.Fake<Func<string, Task<Option<FakeEntity>>>>();
		}

		[TestCase("123", true)]
		[TestCase("", false)]
		[TestCase("asd", false)]
		public void Should_AbleToValidateSearchString_Using_DefaultValidation(string searchString, bool expected)
		{
			// Arrange
			EntitySearcher<FakeEntity> searcher = new EntitySearcher<FakeEntity>(_onSearch);
			searcher.SearchString = searchString;

			// Act

			// Assert
			Assert.That(searcher.SearchCommand.CanExecute(null), Is.EqualTo(expected));
		}

		[TestCase("123abc", true)]
		[TestCase("", false)]
		[TestCase("asd", false)]
		public void Should_AbleToValidateSearchString_Using_CustomValidation(string searchString, bool expected)
		{
			// Arrange
			EntitySearcher<FakeEntity> searcher = new EntitySearcher<FakeEntity>(_onSearch, FakeCanSearch);
			searcher.SearchString = searchString;

			// Act

			// Assert
			Assert.That(searcher.SearchCommand.CanExecute(null), Is.EqualTo(expected));

			bool FakeCanSearch(string search)
			{
				return search.Length > 0 && search.Contains("123abc");
			}
		}

		[Test]
		public void Should_SetIsFoundToTrue()
		{
			// Arrange
			EntitySearcher<FakeEntity> searcher = new EntitySearcher<FakeEntity>(_onSearch);
			A.CallTo(() => _onSearch.Invoke(A<string>._))
				.Returns(Option.Some(new FakeEntity()
				{
					Id = 123
				}));

			// Act
			searcher.SearchString = "123";
			searcher.SearchCommand.Execute(null);

			// Assert
			Assert.That(searcher.IsFound.Contains(true), Is.True);
		}

		[Test]
		public void Should_SetIsFoundToFalse()
		{
			// Arrange

			EntitySearcher<FakeEntity> searcher = new EntitySearcher<FakeEntity>(_onSearch);

			A.CallTo(() => _onSearch.Invoke(A<string>._)).Returns(Option.None<FakeEntity>());

			// Act
			searcher.SearchString = "123";
			searcher.SearchCommand.Execute(null);

			// Assert
			Assert.That(searcher.IsFound.Contains(false), Is.True);
		}

		[Test]
		public void Should_SetIsFoundToNone_AtStartup()
		{
			// Arrange
			EntitySearcher<FakeEntity> searcher = new EntitySearcher<FakeEntity>(_onSearch);


			// Act


			// Assert
			Assert.That(searcher.IsFound.HasValue, Is.False);
		}

		[Test]
		public void Should_SetIsFoundToNone_If_SearchStringChanges()
		{
			// Arrange
			EntitySearcher<FakeEntity> searcher = new EntitySearcher<FakeEntity>(_onSearch);
			searcher.SearchString = "123";

			// emulate search

			searcher.SearchCommand.Execute(null);

			// Act
			searcher.SearchString = "Changed";

			// Assert
			Assert.That(searcher.IsFound.HasValue, Is.False);
		}


		[Test]
		public void Should_RaiseCanExecuteChange_If_SearchStringChanges()
		{
			// Arrange
			EntitySearcher<FakeEntity> searcher = new EntitySearcher<FakeEntity>(_onSearch);
			var eventHandler = A.Fake<EventHandler>();
			searcher.SearchCommand.CanExecuteChanged += eventHandler;

			// Act
			searcher.SearchString = "Changed";

			// Assert
			A.CallTo(eventHandler).MustHaveHappened();
		}

		[Test]
		public void Should_InvokeCallback_After_Search()
		{
			// Arrange
			var callBack = A.Fake<Action>();
			EntitySearcher<FakeEntity> searcher = new EntitySearcher<FakeEntity>(_onSearch, onSearched: callBack);
			A.CallTo(() => _onSearch.Invoke(A<string>._))
				.Returns(Option.None<FakeEntity>());

			searcher.SearchString = "123";

			// Act
			searcher.SearchCommand.Execute(null);

			// Assert
			A.CallTo(() => callBack.Invoke()).MustHaveHappened();
		}
	}
}