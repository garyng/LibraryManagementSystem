using System.Threading.Tasks;
using Autofac;
using Libraryman.Common.Result;
using Libraryman.Wpf.Command;
using Libraryman.Wpf.Query;
using NUnit.Framework;

namespace Libraryman.Wpf.Tests
{
	[Category("Integration Test")]
	[TestFixture]
	public class QueryDispatcherTests
	{
		// should able to execute query
		// should able to resolve handler
		// should able to return result

		private class GetFakeUserById : IQuery
		{
			public int Id { get; set; }
		}

		private class FakeUser
		{
			public int Id { get; set; }
			public string Name { get; set; }
		}


		private class GetFakeUserByIdQueryHandler : IAsyncQueryHandler<GetFakeUserById, Result<FakeUser>>
		{
			public Task<Result<FakeUser>> HandleAsync(GetFakeUserById query)
			{
				return Task.FromResult(Result.Ok(new FakeUser()
				{
					Id = query.Id,
					Name = "Testing"
				}));
			}
		}

		[Test]
		public async Task Should_CorrectlyCallQueryHandler()
		{
			// Arrange
			ContainerBuilder cb = new ContainerBuilder();

			cb.RegisterType<CommandQueryResolver>()
				.As<ICommandQueryResolver>()
				.SingleInstance();

			cb.RegisterType<AsyncQueryDispatcher>()
				.As<IAsyncQueryDispatcher>()
				.SingleInstance();

			cb.RegisterType<GetFakeUserByIdQueryHandler>()
				.As<IAsyncQueryHandler<GetFakeUserById, Result<FakeUser>>>();

			IContainer container = cb.Build();

			// Act
			GetFakeUserById query = new GetFakeUserById() {Id = 1};
			Result<FakeUser> result = await container.Resolve<IAsyncQueryDispatcher>()
				.DispatchAsync<GetFakeUserById, Result<FakeUser>>(query)
				.ConfigureAwait(false);

			// Assert
			Assert.That(result.IsSuccess, Is.True);
			Assert.That(result.Value.Id, Is.EqualTo(1));
			Assert.That(result.Value.Name, Is.EqualTo("Testing"));
		}
	}
}