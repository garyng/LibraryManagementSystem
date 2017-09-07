namespace Libraryman.Wpf.Service
{
	// todo: does it need to be observable?
	public class AuthenticationState
	{
		public bool IsLoggedIn { get; set; }

		public bool IsLoggedOut
		{
			get => !IsLoggedIn;
		}

		public int StaffId { get; set; }
	}
}