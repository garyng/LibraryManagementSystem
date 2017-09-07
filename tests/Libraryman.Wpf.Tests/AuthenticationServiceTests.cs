using System;
using System.Threading.Tasks;
using FakeItEasy;
using Libraryman.Common.Extensions;
using Libraryman.Common.Result;
using Libraryman.Entity;
using Libraryman.Wpf.Command;
using Libraryman.Wpf.Extensions;
using Libraryman.Wpf.Query;
using Libraryman.Wpf.Service;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Libraryman.Wpf.Tests
{
	[Category("Unit Test")]
	[TestFixture]
	public class AuthenticationServiceTests
	{
		// should fail if staff not found w/ message
		// should fail if wrong password w/ message
		// should ok if correct staff id + password

		private IAsyncQueryDispatcher _queryDispatcher;
		private IAsyncCommandDispatcher _commandDispatcher;
		private AuthenticationService _auth;

		[SetUp]
		public void Setup()
		{
			_queryDispatcher = A.Fake<IAsyncQueryDispatcher>();
			_commandDispatcher = A.Fake<IAsyncCommandDispatcher>();
			_auth = new AuthenticationService(_commandDispatcher, _queryDispatcher);
		}

		[Test]
		public async Task Should_Authenticate_When_StaffIdAndPasswordIsCorrect()
		{
			// Arrange
			Staff staff = new Staff()
			{
				Id = 1000,
				PasswordHash = "hash".ToSHA256(),
			};

			A.CallTo(() => _queryDispatcher.DispatchAsync<GetStaffById, Result<Staff>>(A<GetStaffById>._))
				.Returns(Result.Ok(staff));

			// Act
			Result<bool> result = await _auth.AuthenticateAsync(1000, "hash".ConvertFromString()).ConfigureAwait(false);

			// Assert
			Assert.That(result.IsSuccess, Is.True);
		}

		[Test]
		public async Task Should_NotAuthenticate_When_StaffIdDoesntExist()
		{
			// Arrange
			string errorMessage = "Staff with id -1 not found.";
			A.CallTo(() => _queryDispatcher.DispatchAsync<GetStaffById, Result<Staff>>(A<GetStaffById>._))
				.Returns(Result.Fail<Staff>(errorMessage));

			// Act
			Result<bool> result = await _auth.AuthenticateAsync(-1, "hash".ConvertFromString()).ConfigureAwait(false);

			// Assert
			Assert.That(result.IsFailure, Is.True);
			Assert.That(result.Error, Is.EqualTo(errorMessage));
		}

		[Test]
		public async Task Should_NotAuthenticate_When_StaffPasswordIsIncorrect()
		{
			// Arrange
			Staff staff = new Staff()
			{
				Id = 1000,
				PasswordHash = "hash".ToSHA256(),
			};

			A.CallTo(() => _queryDispatcher.DispatchAsync<GetStaffById, Result<Staff>>(A<GetStaffById>._))
				.Returns(Result.Ok(staff));

			// Act
			Result<bool> result = await _auth.AuthenticateAsync(1000, "wrong hash".ConvertFromString()).ConfigureAwait(false);


			// Assert
			Assert.That(result.IsFailure, Is.True);
			Assert.That(result.Error, Is.EqualTo("Wrong password!"));
		}
	}
}