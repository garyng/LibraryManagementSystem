using System;

namespace Libraryman.Common.Result
{
	public static class ResultExtensions
	{
		/// <summary>
		/// Maps <see cref="Result{T}"/> with value of type <code>T</code> to <see cref="Result{T}"/> with value of type <code>K</code> and wrap it in a <code>Result.Ok</code>.
		/// <para>-else-</para>
		/// Returns a <code>Result.Fail</code> if the <paramref name="result"/> was failed.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="K"></typeparam>
		/// <param name="result"></param>
		/// <param name="func"></param>
		/// <returns></returns>
		public static Result<K> OnSuccess<T, K>(this Result<T> result, Func<T, K> func)
		{
			return result.Match(
				onSuccess: v => Result.Ok(func(v)),
				onFailure: r => Result.Fail<T, K>(r));
		}

		/// <summary>
		/// Maps <see cref="Result{T}"/> with value of type <code>T</code> to <see cref="Result{T}"/> with value of type <code>T</code> and wrap it in a <code>Result.Ok</code>.
		/// <para>-else-</para>
		/// Returns a <code>Result.Fail</code> if the <paramref name="result"/> was failed.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="result"></param>
		/// <param name="func"></param>
		/// <returns></returns>
		public static Result<T> OnSuccess<T>(this Result<T> result, Func<T> func)
		{
			// more or less similar to: on success then do something that returns T.

			return result.Match<T>(
				onSuccess: v => Result.Ok(func()),
				onFailure: r => Result.Fail(result));
		}

		/// <summary>
		/// Maps <see cref="Result"/> with no internal value to <see cref="Result{T}"/> with value of type <code>T</code> and wrap it in a <code>Result.Ok</code>.
		/// <para>-else-</para>
		/// Returns a <code>Result.Fail</code> if the <paramref name="result"/> was failed.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="result"></param>
		/// <param name="func"></param>
		/// <returns></returns>
		public static Result<T> OnSuccess<T>(this Result result, Func<T> func)
		{
			// more or less similar to: on success then do something that returns T.

			return result.Match(
				onSuccess: r => Result.Ok(func()),
				onFailure: r => Result.Fail<T>(r));
		}

		/// <summary>
		/// Maps a <code>Result&lt;T&gt;</code> to <code>Result&lt;K&gt;</code>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="result"></param>
		/// <param name="func"></param>
		/// <returns></returns>
		public static Result<K> OnSuccess<T, K>(this Result<T> result, Func<T, Result<K>> func)
		{
			return result.Match(
				onSuccess: v => func(v),
				onFailure: r => Result.Fail<T, K>(r));
		}

		/// <summary>
		/// Maps a <code>Result&lt;T&gt;</code> to <code>Result&lt;K&gt;</code>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="K"></typeparam>
		/// <param name="result"></param>
		/// <param name="func"></param>
		/// <returns></returns>
		public static Result<K> OnSuccess<T, K>(this Result<T> result, Func<Result<K>> func)
		{
			return result.Match(
				onSuccess: v => func(),
				onFailure: r => Result.Fail<T, K>(r));
		}

		/// <summary>
		/// Maps <see cref="Result"/> with no internal value to <see cref="Result{T}"/> with value of type <code>T</code>.
		/// <para>-else-</para>
		/// Returns a <code>Result.Fail</code> if the <paramref name="result"/> was failed.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="result"></param>
		/// <param name="func"></param>
		/// <returns></returns>
		public static Result<T> OnSuccess<T>(this Result result, Func<Result<T>> func)
		{
			return result.Match(
				onSuccess: r => func(),
				onFailure: r => Result.Fail<T>(result));
		}

		/// <summary>
		/// Maps <see cref="Result{T}"/> with value of type <code>T</code> to the non generic <see cref="Result"/>.
		/// <para>-else-</para>
		/// Returns a <code>Result.Fail</code> if the <paramref name="result"/> was failed.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="result"></param>
		/// <param name="func"></param>
		/// <returns></returns>
		public static Result OnSuccess<T>(this Result<T> result, Func<T, Result> func)
		{
			return result.Match(
				onSuccess: r => func(r),
				onFailure: r => Result.Fail((Result) r));
		}

		/// <summary>
		/// Maps <see cref="Result"/> to <see cref="Result"/>.
		/// </summary>
		/// <param name="result"></param>
		/// <param name="func"></param>
		/// <returns></returns>
		public static Result OnSuccess(this Result result, Func<Result> func)
		{
			return result.Match(
				onSuccess: r => func(),
				onFailure: r => Result.Fail((Result) r));
		}

		/// <summary>
		/// Execute the <paramref name="action"/>
		/// <para>-and-</para>
		/// Returns the original <paramref name="result"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="result"></param>
		/// <param name="action"></param>
		/// <returns></returns>
		public static Result<T> OnSuccess<T>(this Result<T> result, Action<T> action)
		{
			if (result.IsSuccess)
			{
				action(result.Value);
			}
			return result;
		}

		/// <summary>
		/// Execute the <paramref name="action"/>
		/// <para>-and-</para>
		/// Returns the original <paramref name="result"/>.
		/// </summary>
		/// <param name="result"></param>
		/// <param name="action"></param>
		/// <returns></returns>
		public static Result OnSuccess(this Result result, Action action)
		{
			if (result.IsSuccess)
			{
				action();
			}
			return result;
		}

		/// <summary>
		/// Returns <code>Result.Fail</code> if either the original <paramref name="result"/> was failed or the <paramref name="predicate"/> returns false.
		/// <para>-else-</para>
		/// Returns <code>Result.Ok</code>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="result"></param>
		/// <param name="predicate"></param>
		/// <param name="errorMessage"></param>
		/// <returns></returns>
		public static Result<T> Ensure<T>(this Result<T> result, Func<T, bool> predicate, string errorMessage)
		{
			if (result.IsFailure)
			{
				return Result.Fail(result);
			}
			if (!predicate(result.Value))
			{
				return Result.Fail<T>(errorMessage);
			}
			return Result.Ok(result);
		}

		/// <summary>
		/// Returns <code>Result.Fail</code> if either the original <paramref name="result"/> was failed or the <paramref name="predicate"/> returns false.
		/// <para>-else-</para>
		/// Returns <code>Result.Ok</code>.
		/// </summary>
		/// <param name="result"></param>
		/// <param name="predicate"></param>
		/// <param name="errorMessage"></param>
		/// <returns></returns>
		public static Result Ensure(this Result result, Func<bool> predicate, string errorMessage)
		{
			if (result.IsFailure)
			{
				return Result.Fail(result);
			}
			if (!predicate())
			{
				return Result.Fail(errorMessage);
			}
			return Result.Ok();
		}

		/// <summary>
		/// Maps <see cref="Result{T}"/> with value of type <code>T</code> to <see cref="Result{T}"/> with value of type <code>K</code> and wrap it in a <code>Result.Ok</code>.
		/// <para>-else-</para>
		/// Returns a <code>Result.Fail</code> if the <paramref name="result"/> was failed.
		/// <para>See also <seealso cref="OnSuccess{T,K}(Results.Result{T},System.Func{T,K})"/>.</para>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="K"></typeparam>
		/// <param name="result"></param>
		/// <param name="func"></param>
		/// <returns></returns>
		public static Result<K> Map<T, K>(this Result<T> result, Func<T, K> func)
		{
			return result.Match(
				onSuccess: v => Result.Ok(func(v)),
				onFailure: r => Result.Fail<T, K>(r));
		}

		/// <summary>
		/// Maps <see cref="Result"/> with no internal value to <see cref="Result{T}"/> with value of type <code>T</code> and wrap it in a <code>Result.Ok</code>.
		/// <para>-else-</para>
		/// Returns a <code>Result.Fail</code> if the <paramref name="result"/> was failed.
		/// <para>See also <seealso cref="OnSuccess{T}(Results.Result,System.Func{T})"/></para>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="result"></param>
		/// <param name="func"></param>
		/// <returns></returns>
		public static Result<T> Map<T>(this Result result, Func<T> func)
		{
			return result.Match(
				onSuccess: r => Result.Ok(func()),
				onFailure: r => Result.Fail<T>(r));
		}

		public static T OnBoth<T>(this Result result, Func<Result, T> func)
		{
			return func(result);
		}

		public static K OnBoth<T, K>(this Result<T> result, Func<Result<T>, K> func)
		{
			return func(result);
		}

		/// <summary>
		/// Execute the <paramref name="action"/>
		/// <para>-and-</para>
		/// Returns the original <paramref name="result"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name=""></param>
		/// <param name="action"></param>
		/// <returns></returns>
		public static Result<T> OnFailure<T>(this Result<T> result, Action action)
		{
			if (result.IsFailure)
			{
				action();
			}
			return result;
		}

		/// <summary>
		/// Execute the <paramref name="action"/>
		/// <para>-and-</para>
		/// Returns the original <paramref name="result"/>.
		/// </summary>
		/// <param name="result"></param>
		/// <param name="action"></param>
		/// <returns></returns>
		public static Result OnFailure(this Result result, Action action)
		{
			if (result.IsFailure)
			{
				action();
			}
			return result;
		}

		/// <summary>
		/// Execute the <paramref name="action"/> with the original <paramref name="result"/> passed in
		/// <para>-and-</para>
		/// Returns the original <paramref name="result"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="result"></param>
		/// <param name="action"></param>
		/// <returns></returns>
		public static Result<T> OnFailure<T>(this Result<T> result, Action<Result<T>> action)
		{
			if (result.IsFailure)
			{
				action(result);
			}
			return result;
		}

		/// <summary>
		/// Execute the <paramref name="action"/> with the original <paramref name="result"/> passed in
		/// <para>-and-</para>
		/// Returns the original <paramref name="result"/>.
		/// </summary>
		/// <param name="result"></param>
		/// <param name="action"></param>
		/// <returns></returns>
		public static Result OnFailure(this Result result, Action<Result> action)
		{
			if (result.IsFailure)
			{
				action(result);
			}
			return result;
		}

		/// <summary>
		/// Execute the <paramref name="action"/> with the (user-friendly) error message of original <paramref name="result"/> passed in
		/// <para>-and-</para>
		/// Returns the original <paramref name="result"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="result"></param>
		/// <param name="action"></param>
		/// <returns></returns>
		public static Result<T> OnFailure<T>(this Result<T> result, Action<string> action)
		{
			if (result.IsFailure)
			{
				action(result.Error);
			}
			return result;
		}

		/// <summary>
		/// Execute the <paramref name="action"/> with the (user-friendly) error message of original <paramref name="result"/> passed in
		/// <para>-and-</para>
		/// Returns the original <paramref name="result"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="result"></param>
		/// <param name="action"></param>
		/// <returns></returns>
		public static Result OnFailure(this Result result, Action<string> action)
		{
			if (result.IsFailure)
			{
				action(result.Error);
			}
			return result;
		}

		public static Result Match(this Result result,
			Func<Result, Result> onSuccess,
			Func<Result, Result> onFailure)
		{
			return result.IsSuccess ? onSuccess(result) : onFailure(result);
		}

		public static Result<T> Match<T>(this Result result,
			Func<Result, Result<T>> onSuccess,
			Func<Result, Result<T>> onFailure)
		{
			return result.IsSuccess ? onSuccess(result) : onFailure(result);
		}

		public static Result Match<T>(this Result<T> result,
			Func<T, Result> onSuccess,
			Func<Result, Result> onFailure)
		{
			return result.IsSuccess ? onSuccess(result.Value) : onFailure(result);
		}

		public static Result<T> Match<T>(this Result<T> result,
			Func<T, Result<T>> onSuccess,
			Func<Result<T>, Result<T>> onFailure)
		{
			return result.IsSuccess ? onSuccess(result.Value) : onFailure(result);
		}

		public static Result<K> Match<T, K>(this Result<T> result,
			Func<T, Result<K>> onSuccess,
			Func<Result<T>, Result<K>> onFailure)
		{
			return result.IsSuccess ? onSuccess(result.Value) : onFailure(result);
		}
	}
}