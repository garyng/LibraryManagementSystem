using System;
using System.Collections.Specialized;
using System.Security;
using System.Threading.Tasks;
using Bogus;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Libraryman.Wpf.Extensions;
using Libraryman.Wpf.Navigation;

namespace Libraryman.Wpf.Login
{
	public class LoginViewModel : ViewModelBase
	{
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


		public RelayCommand LoginCommand { get; set; }

		public LoginViewModel()
		{
			LoginCommand = new RelayCommand(async () => await Login().ConfigureAwait(false), CanLogin);
			IsLoginSuccessful = true;
#if DEBUG
			if (IsInDesignMode)
			{
				StaffId = "123123";
			}
#endif
		}

		private async Task Login()
		{

		}

		private bool CanLogin()
		{
			return int.TryParse(_staffId, out _) && _staffPassword?.Length > 0;
		}
	}
}