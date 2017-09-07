using System;

namespace Libraryman.Wpf.Command
{
	public class UpdateLastLoginTimeByStaffId : ICommand
	{
		public int Id { get; set; }
		public DateTime LastLogin { get; set; } = DateTime.Now;
	}
}