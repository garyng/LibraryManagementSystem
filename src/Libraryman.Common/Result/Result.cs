using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Libraryman.Common.Extensions;

namespace Libraryman.Common.Result
{
	internal sealed class ResultCommonLogic
	{
		private const string ErrorSeparator = " --> ";
		public bool IsSuccess { get; }
		public bool IsFailure => !IsSuccess;

		private readonly string _error;
		private readonly string _detailedError;
		private readonly ImmutableList<string> _errorsList;

		/// <summary>
		/// The topmost error of all errors occurred. This property is meant to be displayed to the user.
		/// </summary>
		/// <exception cref="InvalidOperationException" accessor="get">There is no error message for succeeded result.</exception>
		public string Error
		{
			get
			{
				if (IsSuccess)
				{
					throw new InvalidOperationException("There is no error message for succeeded result.");
				}
				return _error;
			}
		}

		/// <summary>
		/// A "stack trace"-like error messages.
		/// <para>-example-</para>
		/// Something went wrong. --> There is an error while making web request. --> [System.Exception: ...]
		/// </summary>
		/// <exception cref="InvalidOperationException" accessor="get">There is no error message for succeeded result.</exception>
		public string DetailedError
		{
			get
			{
				if (IsSuccess)
				{
					throw new InvalidOperationException("There is no error message for succeeded result.");
				}
				return _detailedError;
			}
		}

		public ImmutableList<string> AllErrors
		{
			get
			{
				if (IsSuccess)
				{
					throw new InvalidOperationException("There is no error message for succeeded result.");
				}
				return _errorsList;
			}
		}

		/// <exception cref="ArgumentException">There should be no error messages for a succeeded result.</exception>
		/// <exception cref="ArgumentNullException">There must be some error messages for a failed result. </exception>
		public ResultCommonLogic(bool isSuccess, string[] errors)
		{
			if (isSuccess)
			{
				if (errors != null)
				{
					throw new ArgumentException("There should be no error messages for a succeeded result.", nameof(errors));
				}
				_errorsList = null;
				_detailedError = null;
			}
			else
			{
				// there should not have any null value
				if (errors == null || errors.Length == 0 || errors.Any(string.IsNullOrEmpty))
				{
					throw new ArgumentNullException(nameof(errors), "There must be some error messages for a failed result.");
				}
				_errorsList = ImmutableList.Create(errors);
				_detailedError = string.Join(ErrorSeparator, _errorsList);
			}

			IsSuccess = isSuccess;
			_error = _errorsList?.First();
		}
	}

	public struct Result
	{
		private static readonly Result OkResult = new Result(true, null);
		private readonly ResultCommonLogic _logic;

		public bool IsSuccess => _logic.IsSuccess;
		public bool IsFailure => _logic.IsFailure;
		public string Error => _logic.Error;
		public string DetailedError => _logic.DetailedError;
		private IEnumerable<string> AllErrors => _logic.AllErrors;


		private Result(bool isSuccess, string[] errors)
		{
			_logic = new ResultCommonLogic(isSuccess, errors);
		}

		public static Result Ok()
		{
			return OkResult;
		}

		public static Result Fail(string error)
		{
			return new Result(false, new[] { error });
		}

		public static Result Fail(string error, string innerError)
		{
			return new Result(false, new[] { error, innerError });
		}

		public static Result Fail(string error, Exception innerException)
		{
			return new Result(false, new[] { error, innerException?.ToString() });
		}

		private static Result Fail(string[] errors)
		{
			return new Result(false, errors);
		}

		public static Result Fail(Result originalResult)
		{
			return new Result(false, originalResult.AllErrors.ToArray());
		}

		public static Result Fail(string error, Result originalResult)
		{
			return new Result(false,
				originalResult.AllErrors
					.Prepend(error)
					.ToArray());
		}

		public static Result<T> Ok<T>(T value)
		{
			return new Result<T>(true, value, null);
		}

		public static Result<T> Ok<T>(Result<T> originalResult)
		{
			return new Result<T>(true, originalResult.Value, null);
		}

		public static Result<T> Fail<T>(string error)
		{
			return new Result<T>(false, default(T), new[] { error });
		}

		public static Result<T> Fail<T>(string error, string innerError)
		{
			return new Result<T>(false, default(T), new[] { error, innerError });
		}

		public static Result<T> Fail<T>(string error, Exception innerException)
		{
			return new Result<T>(false, default(T), new[] { error, innerException?.ToString() });
		}

		public static Result<T> Fail<T>(Result originalResult)
		{
			return new Result<T>(false, default(T), originalResult.AllErrors.ToArray());
		}

		public static Result<T> Fail<T>(Result<T> originalResult)
		{
			return new Result<T>(false, default(T), originalResult.AllErrors.ToArray());
		}

		public static Result<T> Fail<T>(string errorMessage, Result<T> originalResult)
		{
			return new Result<T>(false, default(T),
				originalResult.AllErrors
					.Prepend(errorMessage)
					.ToArray());
		}

		public static Result<K> Fail<T, K>(Result<T> originalResult)
		{
			return new Result<K>(false, default(K), originalResult.AllErrors.ToArray());
		}


		/// <summary>
		/// Returns the first failure in <paramref name="results"/>. If there is no failure returns.
		/// </summary>
		/// <param name="results"></param>
		/// <returns></returns>
		public static Result FirstFailureElseSuccess(params Result[] results)
		{
			foreach (Result result in results)
			{
				if (result.IsFailure)
				{
					return Fail(result.AllErrors.ToArray());
				}
			}
			return Ok();
		}
	}

	public struct Result<T>
	{
		private readonly ResultCommonLogic _logic;
		private readonly T _value;

		public bool IsSuccess => _logic.IsSuccess;
		public bool IsFailure => _logic.IsFailure;
		public string Error => _logic.Error;
		public string DetailedError => _logic.DetailedError;
		internal IEnumerable<string> AllErrors => _logic.AllErrors;


		/// <exception cref="InvalidOperationException" accessor="get">There is no value for failed result.</exception>
		public T Value
		{
			get
			{
				if (IsFailure)
				{
					throw new InvalidOperationException("There is no value for failed result.");
				}
				return _value;
			}
		}

		/// <exception cref="ArgumentNullException">There must be a <paramref name="value"/> for succeeded result. </exception>
		internal Result(bool isSuccess, T value, string[] errors)
		{
			if (isSuccess && value == null)
			{
				throw new ArgumentNullException(nameof(value), "There must be a value for succeeded result.");
			}
			_logic = new ResultCommonLogic(isSuccess, errors);
			_value = value;
		}

		public static implicit operator Result(Result<T> result)
		{
			if (result.IsSuccess)
			{
				return Result.Ok();
			}
			else
			{
				return Result.Fail(result.Error);
			}
		}
	}
}