using System;
using System.Threading.Tasks;

namespace Libraryman.Common.Result
{
	/// <summary>
	/// Extensions for async operations where Task appears in the right operand.
	/// </summary>
	public static class AsyncResultExtensionsRightOperand
	{
		public static async Task<Result<K>> OnSuccess<T, K>(this Result<T> result, Func<T, Task<K>> func)
		{
			return await result.Match(
					onSuccess: async v => Result.Ok(await func(v).ConfigureAwait(false)),
					onFailure: r => Result.Fail<K>(r))
				.ConfigureAwait(false);
		}

		public static async Task<Result<T>> OnSuccess<T>(this Result<T> result, Func<Task<T>> func)
		{
			return await result.Match<T>(
					onSuccess: async v => Result.Ok(await func().ConfigureAwait(false)),
					onFailure: r => Result.Fail(r))
				.ConfigureAwait(false);
		}

		public static async Task<Result<T>> OnSuccess<T>(this Result result, Func<Task<T>> func)
		{
			return await result.Match(
					onSuccess: async r => Result.Ok(await func().ConfigureAwait(false)),
					onFailure: r => Result.Fail<T>(r))
				.ConfigureAwait(false);
		}

		public static async Task<Result<K>> OnSuccess<T, K>(this Result<T> result, Func<T, Task<Result<K>>> func)
		{
			return await result.Match(
					onSuccess: async v => await func(v).ConfigureAwait(false),
					onFailure: r => Result.Fail<T, K>(r))
				.ConfigureAwait(false);
		}

		public static async Task<Result<K>> OnSuccess<T, K>(this Result<T> result, Func<Task<Result<K>>> func)
		{
			return await result.Match(
					onSuccess: async v => await func().ConfigureAwait(false),
					onFailure: r => Result.Fail<K>(r))
				.ConfigureAwait(false);
		}

		public static async Task<Result<T>> OnSuccess<T>(this Result result, Func<Task<Result<T>>> func)
		{
			return await result.Match(
				onSuccess: async r => await func().ConfigureAwait(false),
				onFailure: r => Result.Fail<T>(r));
		}

		public static async Task<Result> OnSuccess(this Result result, Func<Task<Result>> func)
		{
			return await result.Match(
					onSuccess: async r => await func().ConfigureAwait(false),
					onFailure: r => Task.FromResult(Result.Fail((Result) r)))
				.ConfigureAwait(false);
		}

		public static async Task<Result<T>> OnSuccess<T>(this Result<T> result, Func<T, Task> action)
		{
			if (result.IsSuccess)
			{
				await action(result.Value).ConfigureAwait(false);
			}
			return result;
		}

		public static async Task<Result> OnSuccess(this Result result, Func<Task> action)
		{
			if (result.IsSuccess)
			{
				await action().ConfigureAwait(false);
			}
			return result;
		}

		public static async Task<Result<T>> Ensure<T>(this Result<T> result, Func<T, Task<bool>> predicate,
			string errorMessage)
		{
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

		public static async Task<Result> Ensure(this Result result, Func<Task<bool>> predicate, string errorMessage)
		{
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

		public static async Task<Result<K>> Map<T, K>(this Result<T> result, Func<T, Task<K>> func)
		{
			return await result.Match(
					onSuccess: async v => Result.Ok(await func(v).ConfigureAwait(false)),
					onFailure: r => Result.Fail<K>(r))
				.ConfigureAwait(false);
		}

		public static async Task<Result<T>> Map<T>(this Result result, Func<Task<T>> func)
		{
			return await result.Match(
					onSuccess: async r => Result.Ok(await func().ConfigureAwait(false)),
					onFailure: r => Result.Fail<T>(r))
				.ConfigureAwait(false);
		}

		public static async Task<T> OnBoth<T>(this Result result, Func<Result, Task<T>> func)
		{
			return await func(result).ConfigureAwait(false);
		}

		public static async Task<K> OnBoth<T, K>(this Result<T> result, Func<Result<T>, Task<K>> func)
		{
			return await func(result).ConfigureAwait(false);
		}

		public static async Task<Result<T>> OnFailure<T>(this Result<T> result, Func<Task> action)
		{
			if (result.IsFailure)
			{
				await action().ConfigureAwait(false);
			}
			return result;
		}

		public static async Task<Result> OnFailure(this Result result, Func<Task> action)
		{
			if (result.IsFailure)
			{
				await action().ConfigureAwait(false);
			}
			return result;
		}

		public static async Task<Result<T>> OnFailure<T>(this Result<T> result, Func<Result<T>, Task> action)
		{
			if (result.IsFailure)
			{
				await action(result).ConfigureAwait(false);
			}
			return result;
		}

		public static async Task<Result> OnFailure(this Result result, Func<Result, Task> action)
		{
			if (result.IsFailure)
			{
				await action(result).ConfigureAwait(false);
			}
			return result;
		}

		public static async Task<Result<T>> OnFailure<T>(this Result<T> result, Func<string, Task> action)
		{
			if (result.IsFailure)
			{
				await action(result.Error).ConfigureAwait(false);
			}
			return result;
		}

		public static async Task<Result> OnFailure(this Result result, Func<string, Task> action)
		{
			if (result.IsFailure)
			{
				await action(result.Error).ConfigureAwait(false);
			}
			return result;
		}

		public static async Task<Result> Match(this Result result,
			Func<Result, Task<Result>> onSuccess,
			Func<Result, Result> onFailure)
		{
			return result.IsSuccess
				? await onSuccess(result).ConfigureAwait(false)
				: onFailure(result);
		}

		public static async Task<Result> Match(this Result result,
			Func<Result, Task<Result>> onSuccess,
			Func<Result, Task<Result>> onFailure)
		{
			return result.IsSuccess
				? await onSuccess(result).ConfigureAwait(false)
				: await onFailure(result).ConfigureAwait(false);
		}

		public static async Task<Result<T>> Match<T>(this Result result,
			Func<Result, Task<Result<T>>> onSuccess,
			Func<Result, Task<Result<T>>> onFailure)
		{
			return result.IsSuccess
				? await onSuccess(result).ConfigureAwait(false)
				: await onFailure(result).ConfigureAwait(false);
		}

		public static async Task<Result<T>> Match<T>(this Result result,
			Func<Result, Task<Result<T>>> onSuccess,
			Func<Result, Result<T>> onFailure)
		{
			return result.IsSuccess
				? await onSuccess(result).ConfigureAwait(false)
				: onFailure(result);
		}

		public static async Task<Result<K>> Match<T, K>(this Result<T> result,
			Func<T, Task<Result<K>>> onSuccess,
			Func<Result<T>, Task<Result<K>>> onFailure)
		{
			return result.IsSuccess
				? await onSuccess(result.Value).ConfigureAwait(false)
				: await onFailure(result).ConfigureAwait(false);
		}

		public static async Task<Result<K>> Match<T, K>(this Result<T> result,
			Func<T, Task<Result<K>>> onSuccess,
			Func<Result<T>, Result<K>> onFailure)
		{
			return result.IsSuccess
				? await onSuccess(result.Value).ConfigureAwait(false)
				: onFailure(result);
		}

		public static async Task<Result<T>> Match<T>(this Result<T> result,
			Func<T, Task<Result<T>>> onSuccess,
			Func<Result<T>, Task<Result<T>>> onFailure)
		{
			return result.IsSuccess
				? await onSuccess(result.Value).ConfigureAwait(false)
				: await onFailure(result).ConfigureAwait(false);
		}

		public static async Task<Result<T>> Match<T>(this Result<T> result,
			Func<T, Task<Result<T>>> onSuccess,
			Func<Result<T>, Result<T>> onFailure)
		{
			return result.IsSuccess
				? await onSuccess(result.Value).ConfigureAwait(false)
				: onFailure(result);
		}
	}
}