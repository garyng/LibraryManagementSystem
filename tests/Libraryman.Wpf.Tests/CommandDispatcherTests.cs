using System.Threading.Tasks;
using Autofac;
using FakeItEasy;
using Libraryman.Common.Result;
using Libraryman.Wpf.Command;
using NUnit.Framework;

namespace Libraryman.Wpf.Tests
{
	[Category("Integration Test")]
	[TestFixture]
	public class CommandDispatcherTests
	{
		// - should able to resolve command handler
		// - return the result of the command?
		// - should call the right handler

		public class FakeUser
		{
			public int Id { get; set; }
			public string Name { get; set; }
		}

		private class CreateFakeUser : ICommand
		{
			public string Name { get; set; }
		}

		public interface IFakeDbContext
		{
			void Add(FakeUser user);
		}

		private class FakeUserCommandHandler : IAsyncCommandHandler<CreateFakeUser, Result<FakeUser>>
		{
			private readonly IFakeDbContext _context;

			public FakeUserCommandHandler(IFakeDbContext context)
			{
				_context = context;
			}

			public Task<Result<FakeUser>> HandleAsync(CreateFakeUser command)
			{
				FakeUser fu = new FakeUser()
				{
					Name = command.Name
				};
				_context.Add(fu);
				return Task.FromResult(Result.Ok(fu));
			}
		}

		[Test]
		public async Task Should_CorrectlyCallCommandHandler()
		{
			// Arrange
			ContainerBuilder cb = new ContainerBuilder();

			cb.RegisterType<CommandQueryResolver>()
				.As<ICommandQueryResolver>()
				.SingleInstance();

			var fakeDbContext = A.Fake<IFakeDbContext>();
			A.CallTo(() => fakeDbContext.Add(A<FakeUser>._))
				.Invokes((FakeUser fu) => fu.Id = 1001);

			cb.Register(c => fakeDbContext)
				.As<IFakeDbContext>();

			cb.RegisterType<FakeUserCommandHandler>()
				.As<IAsyncCommandHandler<CreateFakeUser, Result<FakeUser>>>();

			cb.RegisterType<AsyncCommandDispatcher>()
				.As<IAsyncCommandDispatcher>()
				.SingleInstance();

			IContainer container = cb.Build();

			// Act
			CreateFakeUser command = new CreateFakeUser() {Name = "Testing"};
			var dispatcher = container.Resolve<IAsyncCommandDispatcher>();

			Result<FakeUser> result = await dispatcher.DispatchAsync<CreateFakeUser, Result<FakeUser>>(command)
				.ConfigureAwait(false);


			// Assert
			Assert.That(result.IsSuccess, Is.True);
			Assert.That(result.Value.Id, Is.EqualTo(1001));
		}
	}
}