using GalaSoft.MvvmLight;

namespace Libraryman.Wpf.Service
{
	// todo: does it need to be observable?
	public class AuthenticationState : ObservableObject
	{

		private bool _isLoggedIn;

		public bool IsLoggedIn
		{
			get => _isLoggedIn;
			set => Set(ref _isLoggedIn, value);
		}

		private bool _isLoggedOut;

		public bool IsLoggedOut
		{
			get => _isLoggedOut;
			set => Set(ref _isLoggedOut, value);
		}

		private int _staffId;

		public int StaffId
		{
			get => _staffId;
			set => Set(ref _staffId, value);
		}

		public AuthenticationState()
		{
			IsLoggedIn = false;
			IsLoggedOut = true;
			StaffId = -1;
		}
	}
}