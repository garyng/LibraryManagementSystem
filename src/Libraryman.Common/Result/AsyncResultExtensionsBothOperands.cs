using System;
using System.Threading.Tasks;

namespace Libraryman.Common.Result
{
	/// <summary>
	/// Extensions for async operations where Task appears in the both operands.
	/// </summary>
	public static class AsyncResultExtensionsBothOperands
	{
		public static async Task<Result<K>> OnSuccess<T, K>(this Task<Result<T>> resultTask, Func<T, Task<K>> func)
		{
			return await resultTask.Match(
					onSuccess: async v => Result.Ok(await func(v).ConfigureAwait(false)),
					onFailure: r => Result.Fail<K>(r))
				.ConfigureAwait(false);
		}

		public static async Task<Result<T>> OnSuccess<T>(this Task<Result<T>> resultTask, Func<Task<T>> func)
		{
			return await resultTask.Match<T>(
					onSuccess: async v => Result.Ok(await func().ConfigureAwait(false)),
					onFailure: r => Result.Fail(r))
				.ConfigureAwait(false);
		}

		public static async Task<Result<T>> OnSuccess<T>(this Task<Result> resultTask, Func<Task<T>> func)
		{
			return await resultTask.Match(
					onSuccess: async r => Result.Ok(await func().ConfigureAwait(false)),
					onFailure: r => Result.Fail<T>(r))
				.ConfigureAwait(false);
		}

		public static async Task<Result<K>> OnSuccess<T, K>(this Task<Result<T>> resultTask, Func<T, Task<Result<K>>> func)
		{
			return await resultTask.Match(
					onSuccess: async v => await func(v).ConfigureAwait(false),
					onFailure: r => Result.Fail<T, K>(r))
				.ConfigureAwait(false);
		}

		public static async Task<Result<K>> OnSuccess<T, K>(this Task<Result<T>> resultTask, Func<Task<Result<K>>> func)
		{
			return await resultTask.Match(
					onSuccess: async v => await func().ConfigureAwait(false),
					onFailure: r => Result.Fail<K>(r))
				.ConfigureAwait(false);
		}

		public static async Task<Result<T>> OnSuccess<T>(this Task<Result> resultTask, Func<Task<Result<T>>> func)
		{
			return await resultTask.Match(
				onSuccess: async r => await func().ConfigureAwait(false),
				onFailure: r => Result.Fail<T>(r));
		}

		public static async Task<Result> OnSuccess(this Task<Result> resultTask, Func<Task<Result>> func)
		{
			return await resultTask.Match(
					onSuccess: async r => await func().ConfigureAwait(false),
					onFailure: r => Task.FromResult(Result.Fail((Result) r)))
				.ConfigureAwait(false);
		}

		public static async Task<Result<T>> OnSuccess<T>(this Task<Result<T>> resultTask, Func<T, Task> action)
		{
			Result<T> result = await resultTask.ConfigureAwait(false);
			if (result.IsSuccess)
			{
				await action(result.Value).ConfigureAwait(false);
			}
			return result;
		}

		public static async Task<Result> OnSuccess(this Task<Result> resultTask, Func<Task> action)
		{
			Result result = await resultTask.ConfigureAwait(false);
			if (result.IsSuccess)
			{
				await action().ConfigureAwait(false);
			}
			return result;
		}

		public static async Task<Result<T>> Ensure<T>(this Task<Result<T>> resultTask, Func<T, Task<bool>> predicate,
			string errorMessage)
		{
			Result<T> result = await resultTask.ConfigureAwait(false);

			if (result.IsFailure)
			{
				return Result.Fail(result);
			}
			bool condition = await predicate(result.Value).ConfigureAwait(false);
			if (!condition)
			{
				return Result.Fail<T>(errorMessage);
			}
			return Result.Ok(result);
		}

		public static async Task<Result> Ensure(this Task<Result> resultTask, Func<Task<bool>> predicate, string errorMessage)
		{
			Result result = await resultTask.ConfigureAwait(false);

			if (result.IsFailure)
			{
				return Result.Fail(result);
			}
			bool condition = await predicate().ConfigureAwait(false);
			if (!condition)
			{
				return Result.Fail(errorMessage);
			}
			return Result.Ok();
		}

		public static async Task<Result<K>> Map<T, K>(this Task<Result<T>> resultTask, Func<T, Task<K>> func)
		{
			return await resultTask.Match(
					onSuccess: async v => Result.Ok(await func(v).ConfigureAwait(false)),
					onFailure: r => Result.Fail<K>(r))
				.ConfigureAwait(false);
		}

		public static async Task<Result<T>> Map<T>(this Task<Result> resultTask, Func<Task<T>> func)
		{
			return await resultTask.Match(
					onSuccess: async r => Result.Ok(await func().ConfigureAwait(false)),
					onFailure: r => Result.Fail<T>(r))
				.ConfigureAwait(false);
		}

		public static async Task<T> OnBoth<T>(this Task<Result> resultTask, Func<Result, Task<T>> func)
		{
			return await func(
					await resultTask
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		public static async Task<K> OnBoth<T, K>(this Task<Result<T>> resultTask, Func<Result<T>, Task<K>> func)
		{
			return await func(
					await resultTask
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		public static async Task<Result<T>> OnFailure<T>(this Task<Result<T>> resultTask, Func<Task> action)
		{
			Result<T> result = await resultTask.ConfigureAwait(false);

			if (result.IsFailure)
			{
				await action().ConfigureAwait(false);
			}
			return result;
		}

		public static async Task<Result> OnFailure(this Task<Result> resultTask, Func<Task> action)
		{
			Result result = await resultTask.ConfigureAwait(false);
			if (result.IsFailure)
			{
				await action().ConfigureAwait(false);
			}
			return result;
		}

		public static async Task<Result<T>> OnFailure<T>(this Task<Result<T>> resultTask, Func<Result<T>, Task> action)
		{
			Result<T> result = await resultTask.ConfigureAwait(false);
			if (result.IsFailure)
			{
				await action(result).ConfigureAwait(false);
			}
			return result;
		}

		public static async Task<Result> OnFailure(this Task<Result> resultTask, Func<Result, Task> action)
		{
			Result result = await resultTask.ConfigureAwait(false);
			if (result.IsFailure)
			{
				await action(result).ConfigureAwait(false);
			}
			return result;
		}

		public static async Task<Result<T>> OnFailure<T>(this Task<Result<T>> resultTask, Func<string, Task> action)
		{
			Result<T> result = await resultTask.ConfigureAwait(false);
			if (result.IsFailure)
			{
				await action(result.Error).ConfigureAwait(false);
			}
			return result;
		}

		public static async Task<Result> OnFailure(this Task<Result> resultTask, Func<string, Task> action)
		{
			Result result = await resultTask.ConfigureAwait(false);
			if (result.IsFailure)
			{
				await action(result.Error).ConfigureAwait(false);
			}
			return result;
		}

		public static async Task<Result> Match(this Task<Result> resultTask,
			Func<Result, Task<Result>> onSuccess,
			Func<Result, Result> onFailure)
		{
			Result result = await resultTask.ConfigureAwait(false);
			return result.IsSuccess
				? await onSuccess(result).ConfigureAwait(false)
				: onFailure(result);
		}

		public static async Task<Result> Match(this Task<Result> resultTask,
			Func<Result, Task<Result>> onSuccess,
			Func<Result, Task<Result>> onFailure)
		{
			Result result = await resultTask.ConfigureAwait(false);
			return result.IsSuccess
				? await onSuccess(result).ConfigureAwait(false)
				: await onFailure(result).ConfigureAwait(false);
		}

		public static async Task<Result<T>> Match<T>(this Task<Result> resultTask,
			Func<Result, Task<Result<T>>> onSuccess,
			Func<Result, Task<Result<T>>> onFailure)
		{
			Result result = await resultTask.ConfigureAwait(false);
			return result.IsSuccess
				? await onSuccess(result).ConfigureAwait(false)
				: await onFailure(result).ConfigureAwait(false);
		}

		public static async Task<Result<T>> Match<T>(this Task<Result> resultTask,
			Func<Result, Task<Result<T>>> onSuccess,
			Func<Result, Result<T>> onFailure)
		{
			Result result = await resultTask.ConfigureAwait(false);
			return result.IsSuccess
				? await onSuccess(result).ConfigureAwait(false)
				: onFailure(result);
		}

		public static async Task<Result<K>> Match<T, K>(this Task<Result<T>> resultTask,
			Func<T, Task<Result<K>>> onSuccess,
			Func<Result<T>, Task<Result<K>>> onFailure)
		{
			Result<T> result = await resultTask.ConfigureAwait(false);
			return result.IsSuccess
				? await onSuccess(result.Value).ConfigureAwait(false)
				: await onFailure(result).ConfigureAwait(false);
		}

		public static async Task<Result<K>> Match<T, K>(this Task<Result<T>> resultTask,
			Func<T, Task<Result<K>>> onSuccess,
			Func<Result<T>, Result<K>> onFailure)
		{
			Result<T> result = await resultTask.ConfigureAwait(false);
			return result.IsSuccess
				? await onSuccess(result.Value).ConfigureAwait(false)
				: onFailure(result);
		}

		public static async Task<Result<T>> Match<T>(this Task<Result<T>> resultTask,
			Func<T, Task<Result<T>>> onSuccess,
			Func<Result<T>, Task<Result<T>>> onFailure)
		{
			Result<T> result = await resultTask.ConfigureAwait(false);
			return result.IsSuccess
				? await onSuccess(result.Value).ConfigureAwait(false)
				: await onFailure(result).ConfigureAwait(false);
		}

		public static async Task<Result<T>> Match<T>(this Task<Result<T>> resultTask,
			Func<T, Task<Result<T>>> onSuccess,
			Func<Result<T>, Result<T>> onFailure)
		{
			Result<T> result = await resultTask.ConfigureAwait(false);
			return result.IsSuccess
				? await onSuccess(result.Value).ConfigureAwait(false)
				: onFailure(result);
		}
	}
}