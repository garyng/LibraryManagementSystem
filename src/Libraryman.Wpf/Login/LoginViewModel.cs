using System;
using System.Collections.Specialized;
using System.Security;
using System.Threading.Tasks;
using Bogus;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Libraryman.Common.Result;
using Libraryman.Entity;
using Libraryman.Wpf.Dashboard;
using Libraryman.Wpf.Extensions;
using Libraryman.Wpf.Navigation;
using Libraryman.Wpf.Service;

namespace Libraryman.Wpf.Login
{
	public class LoginViewModel : ViewModelBase
	{
		private readonly IAuthenticationService _authentication;
		private AuthenticationState _as;
		private string _staffId;

		public string StaffId
		{
			get => _staffId;
			set
			{
				Set(ref _staffId, value);
				LoginCommand?.RaiseCanExecuteChanged();
			}
		}

		private SecureString _staffPassword;

		public SecureString StaffPassword
		{
			get => _staffPassword;
			set
			{
				Set(ref _staffPassword, value);
				LoginCommand?.RaiseCanExecuteChanged();
			}
		}

		private bool _isLoginSuccessful;

		public bool IsLoginSuccessful
		{
			get => _isLoginSuccessful;
			set => Set(ref _isLoginSuccessful, value);
		}

		private string _loginErrorMessage;

		public string LoginErrorMessage
		{
			get => _loginErrorMessage;
			set => Set(ref _loginErrorMessage, value);
		}

		public RelayCommand LoginCommand { get; set; }

		public LoginViewModel(INavigationService<ViewModelBase> navigation, IAuthenticationService authentication,
			AuthenticationState @as) : base(navigation)
		{
			_authentication = authentication;
			_as = @as;
			LoginCommand = new RelayCommand(async () => await Login().ConfigureAwait(false), CanLogin);
			IsLoginSuccessful = true;
#if DEBUG
			if (IsInDesignMode)
			{
				StaffId = "123123";
				LoginErrorMessage = "Error message!";
			}
#endif
		}

		private async Task Login()
		{
			int staffId = int.Parse(_staffId);
			Result<bool> result = await _authentication.AuthenticateAsync(staffId, _staffPassword)
				.OnSuccess(r =>
				{
					_as.StaffId = staffId;
					_navigation.GoTo<DashboardViewModel>();
				})
				.OnFailure(e => LoginErrorMessage = e.Error)
				.ConfigureAwait(false);
			IsLoginSuccessful = result.IsSuccess;
			_as.IsLoggedIn = result.IsSuccess;
			_as.IsLoggedOut = result.IsFailure;
		}

		private bool CanLogin()
		{
			return int.TryParse(_staffId, out _) && _staffPassword?.Length > 0;
		}
	}
}