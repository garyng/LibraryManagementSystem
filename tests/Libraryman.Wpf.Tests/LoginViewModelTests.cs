using System.Security;
using System.Threading.Tasks;
using FakeItEasy;
using Libraryman.Common.Result;
using Libraryman.Wpf.Extensions;
using Libraryman.Wpf.Login;
using Libraryman.Wpf.Navigation;
using Libraryman.Wpf.Service;
using NUnit.Framework;

namespace Libraryman.Wpf.Tests
{
	public class LoginViewModelTests
	{
		private INavigationService<ViewModelBase> _navigation;

		private IAuthenticationService _authentication;

		private AuthenticationState _state;
		// - CanLogin is true is staffId is int and staffpassword is not zero length
		// - will set authentication state after successful/unsuccessful login
		// - will set IsLoginSuccessful after login
		// will set LoginErrorMessage if failed

		[SetUp]
		public void Setup()
		{
			_navigation = A.Fake<INavigationService<ViewModelBase>>();
			_authentication = A.Fake<IAuthenticationService>();
			_state = new AuthenticationState();
		}

		[TestCase("1", "hash", true)]
		[TestCase("", "hash", false)]
		[TestCase("", "", false)]
		[TestCase("1", "", false)]
		[TestCase("a", "hash", false)]
		public void Should_AbleToValidateStaffIdAndAPassword(string staffId, string staffPasword, bool expected)
		{
			// Arrange
			var loginViewModel = new LoginViewModel(_navigation, _authentication, _state)
			{
				StaffId = staffId,
				StaffPassword = staffPasword.ConvertFromString()
			};

			// Act
			bool canExecute = loginViewModel.LoginCommand.CanExecute(null);

			// Assert
			Assert.That(canExecute, Is.EqualTo(expected));
		}

		[TestCase(true, true)]
		[TestCase(false, false)]
		public void Should_ReflectLoginResult_In_AuthenticationState(bool returns, bool expected)
		{
			// Arrange
			var loginViewModel = new LoginViewModel(_navigation, _authentication, _state)
			{
				StaffId = "1",
				StaffPassword = "hash".ConvertFromString()
			};
			A.CallTo(() => _authentication.AuthenticateAsync(A<int>._, A<SecureString>._))
				.Returns(returns ? Result.Ok(true) : Result.Fail<bool>("Test"));

			// Act
			loginViewModel.LoginCommand.Execute(null);

			// Assert
			Assert.That(loginViewModel.IsLoginSuccessful, Is.EqualTo(expected));
			Assert.That(_state.IsLoggedIn, Is.EqualTo(expected));
			Assert.That(_state.IsLoggedOut, Is.Not.EqualTo(expected));
		}

		[Test]
		public void Should_SetErrorMessage_If_LoginFailed()
		{
			// Arrange
			var loginViewModel = new LoginViewModel(_navigation, _authentication, _state)
			{
				StaffId = "1",
				StaffPassword = "hash".ConvertFromString()
			};
			A.CallTo(() => _authentication.AuthenticateAsync(A<int>._, A<SecureString>._))
				.Returns(Result.Fail<bool>("Testing"));

			// Act
			loginViewModel.LoginCommand.Execute(null);

			// Assert
			Assert.That(loginViewModel.LoginErrorMessage, Is.EqualTo("Testing"));
		}
	}
}