using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using Libraryman.Wpf.Navigation;
using NUnit.Framework;

namespace Libraryman.Wpf.Tests
{
	[Category("Unit Test")]
	[TestFixture]
	public class NavigationServiceTests
	{
		// - should able to register as subscriber/navigation host
		// should receive notification when navigated => implemented through callbacks

		// - should able to register viewmodel/navigation target
		// - should not able to register repeating viewmodel/navigation target
		// - should able to go back
		// - should able to navigate to a viewmodel


		public class FakeViewModelBase : INavigationTarget
		{
		}

		private INavigationHost<FakeViewModelBase> _navigationHost;
		private NavigationService<FakeViewModelBase> _navigationService;

		[SetUp]
		public void Setup()
		{
			_navigationHost = A.Fake<INavigationHost<FakeViewModelBase>>();
			_navigationService = new NavigationService<FakeViewModelBase>(_navigationHost);
		}

		public class ViewModel1 : FakeViewModelBase
		{
			public string Dummy { get; set; }
		}

		public class ViewModel2 : FakeViewModelBase
		{
		}

		public class ViewModel3 : FakeViewModelBase
		{
			public string Dummy { get; set; }
		}

		[Test]
		public void Should_AbleToGoToSpecificViewModel()
		{
			// Arrange
			var viewModel1 = new ViewModel1();
			var viewModel2 = new ViewModel2();
			var viewModel3 = new ViewModel3();
			_navigationService.Register(viewModel1);
			_navigationService.Register(viewModel2);
			_navigationService.Register(viewModel3);

			// Act
			_navigationService.GoTo<ViewModel1>();

			// Assert
			A.CallTo(() => _navigationHost.SetCurrentViewModel(viewModel1)).MustHaveHappened();
		}

		[Test]
		public void Should_NotNavigateIfTargetIsNotRegistered()
		{
			// if a viewmodel requests to navigate to a non-registered viewmodel, 
			// navigation service should just remian at the same viewmodel
			// (imo) without throwing exception

			// Arrange
			var viewModel1 = new ViewModel1();
			_navigationService.Register(viewModel1);


			// Act
			_navigationService.GoTo<ViewModel1>();
			_navigationService.GoTo<ViewModel2>();

			// Assert
			A.CallTo(() => _navigationHost.SetCurrentViewModel(A<FakeViewModelBase>._))
				.MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void Should_CallSetupOnTarget_When_NavigatedWithGoTo()
		{
			// Arrange
			var viewModel1 = new ViewModel1();
			_navigationService.Register(viewModel1);

			// Act
			_navigationService.GoTo<ViewModel1>(vm => vm.Dummy = "Testing");

			// Assert
			Assert.That(viewModel1.Dummy, Is.EqualTo("Testing"));
		}

		[Test]
		public void Should_AbleToGotoSpecificInstanceOfViewModel()
		{
			// Arrange
			var viewModel1 = new ViewModel1();
			var viewModel2 = new ViewModel2();
			var viewModel3 = new ViewModel3();

			_navigationService.Register(viewModel1);
			_navigationService.Register(viewModel2);
			_navigationService.Register(viewModel3);

			// Act
			_navigationService.GoTo(viewModel1);

			// Assert
			A.CallTo(() => _navigationHost.SetCurrentViewModel(viewModel1))
				.MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void Should_NotAbleToGoToNotRegisteredViewModelInstance()
		{
			// Arrange
			var viewModel1 = new ViewModel1();
			var viewModel2 = new ViewModel2();

			_navigationService.Register(viewModel1);

			// Act
			_navigationService.GoTo(viewModel1);
			_navigationService.GoTo(viewModel2);

			// Assert
			A.CallTo(() => _navigationHost.SetCurrentViewModel(A<FakeViewModelBase>._))
				.MustHaveHappened(Repeated.Exactly.Once);
		}

		[Test]
		public void Should_CallSetupOnTarget_When_GoToSpecificInstanceOfViewModel()
		{
			// Arrange
			var viewModel1 = new ViewModel1();
			_navigationService.Register(viewModel1);

			// Act
			_navigationService.GoTo(viewModel1, vm => vm.Dummy = "Testing");

			// Assert
			Assert.That(viewModel1.Dummy, Is.EqualTo("Testing"));
		}

		[Test]
		public void Should_NotAbleToGoBack_When_OnlyNavigatedToOneViewModel()
		{
			// Arrange
			var viewModel1 = new ViewModel1();
			_navigationService.Register(viewModel1);

			// Act
			_navigationService.GoTo<ViewModel1>();

			// Assert
			Assert.That(_navigationService.CanGoBack(), Is.False);
		}

		[Test]
		public void Should_AbleToGoBack()
		{
			// Arrange
			var viewModel1 = new ViewModel1();
			var viewModel2 = new ViewModel2();
			var viewModel3 = new ViewModel3();

			_navigationService.Register(viewModel1);
			_navigationService.Register(viewModel2);
			_navigationService.Register(viewModel3);

			// Act
			_navigationService.GoTo<ViewModel1>();
			_navigationService.GoTo<ViewModel2>();
			// Assert
			Assert.That(_navigationService.CanGoBack(), Is.True);
		}

		[Test]
		public void Should_GoBack()
		{
			// Arrange
			var viewModel1 = new ViewModel1();
			var viewModel2 = new ViewModel2();
			var viewModel3 = new ViewModel3();

			_navigationService.Register(viewModel1);
			_navigationService.Register(viewModel2);
			_navigationService.Register(viewModel3);

			_navigationService.GoTo<ViewModel1>();
			_navigationService.GoTo<ViewModel2>();

			// Act
			_navigationService.GoBack();

			// Assert
			// 1st -> Go to ViewModel1
			// 2nd -> Go back to ViewModel1 from ViewModel2
			A.CallTo(() => _navigationHost.SetCurrentViewModel(viewModel1))
				.MustHaveHappened(Repeated.Exactly.Twice);
		}

		[Test]
		public void Should_CallSetup_When_GoingBack()
		{
			// Arrange
			var viewModel1 = new ViewModel1();
			var viewModel2 = new ViewModel2();
			var viewModel3 = new ViewModel3();

			_navigationService.Register(viewModel1);
			_navigationService.Register(viewModel2);
			_navigationService.Register(viewModel3);

			_navigationService.GoTo<ViewModel1>();
			_navigationService.GoTo<ViewModel2>();

			// Act
			_navigationService.GoBack(vm => ((ViewModel1) vm).Dummy = "Test");

			// Assert
			Assert.That(viewModel1.Dummy, Is.EqualTo("Test"));
		}

		[Test]
		public void Should_CallSetupWithoutCast_When_GoingBack()
		{
			// Arrange
			var viewModel1 = new ViewModel1();
			var viewModel2 = new ViewModel2();
			var viewModel3 = new ViewModel3();

			_navigationService.Register(viewModel1);
			_navigationService.Register(viewModel2);
			_navigationService.Register(viewModel3);

			_navigationService.GoTo<ViewModel1>();
			_navigationService.GoTo<ViewModel2>();

			// Act
			_navigationService.GoBack<ViewModel1>(vm => vm.Dummy = "Test");

			// Assert
			Assert.That(viewModel1.Dummy, Is.EqualTo("Test"));
		}


		[Test]
		public void Should_NotCallSetupWithoutCast_When_GoingBack_And_TypeMismatch()
		{
			// Arrange
			var viewModel1 = new ViewModel1() {Dummy = "ViewModel1"};
			var viewModel2 = new ViewModel2();
			var viewModel3 = new ViewModel3() {Dummy = "ViewModel3"};

			_navigationService.Register(viewModel1);
			_navigationService.Register(viewModel2);
			_navigationService.Register(viewModel3);

			_navigationService.GoTo<ViewModel1>();
			_navigationService.GoTo<ViewModel2>();

			// Act
			_navigationService.GoBack<ViewModel3>(vm => vm.Dummy = "Test");

			// Assert
			Assert.That(viewModel1.Dummy, Is.EqualTo("ViewModel1"));
			Assert.That(viewModel3.Dummy, Is.EqualTo("ViewModel3"));
		}
	}
}